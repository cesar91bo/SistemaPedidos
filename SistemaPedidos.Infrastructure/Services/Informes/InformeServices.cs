using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Informes.DTOs;
using SistemaPedidos.Application.Features.Informes.Interfaces;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Informes
{
    public class InformeService : IInformeService
    {
        private readonly AppDbContext _context;

        public InformeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InformeDashboardDto> ObtenerDashboardAsync(InformeFiltroDto filtro)
        {
            var fechaDesde = filtro.FechaDesde.Date;
            var fechaHasta = filtro.FechaHasta.Date.AddDays(1);

            var ventas = await _context.Ventas
                .Include(v => v.Pedido)
                    .ThenInclude(p => p.Items)
                .Include(v => v.Pedido)
                    .ThenInclude(p => p.Delivery)
                .Include(v => v.Pagos)
                .Where(v => v.FechaVenta >= fechaDesde &&
                            v.FechaVenta < fechaHasta)
                .ToListAsync();

            var totalVentas = ventas.Sum(v => v.TotalGeneral - v.TotalEnvio);
            var cantidadVentas = ventas.Count;

            var pedidosDelivery = ventas.Count(v =>
                v.Pedido != null &&
                v.Pedido.TipoPedido == TipoPedido.Delivery);

            var pedidosRetiro = ventas.Count(v =>
                v.Pedido != null &&
                v.Pedido.TipoPedido == TipoPedido.Retiro);

            var promedio = cantidadVentas > 0
                ? totalVentas / cantidadVentas
                : 0;

            var formasPago = ventas
                .SelectMany(v => v.Pagos.Select(p => new
                {
                    Pago = p,
                    Venta = v
                }))
                .GroupBy(x => x.Pago.FormaPago)
                .Select(g => new FormaPagoInformeDto
                {
                    Nombre = ObtenerNombreFormaPago(g.Key),
                    Cantidad = g.Count(),
                    Total = g.Sum(x =>
                    {
                        var monto = x.Pago.Monto;

                        if (x.Pago.FormaPago == FormaPago.Efectivo &&
                            x.Venta.Pedido?.TipoPedido == TipoPedido.Delivery)
                        {
                            monto -= x.Venta.TotalEnvio;

                            if (monto < 0)
                                monto = 0;
                        }

                        return monto;
                    })
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            var productosMasVendidos = ventas
                .Where(v => v.Pedido != null)
                .SelectMany(v => v.Pedido.Items)
                .GroupBy(i => new
                {
                    i.ProductoId,
                    i.NombreProducto
                })
                .Select(g => new ProductoVendidoDto
                {
                    Nombre = g.Key.NombreProducto,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToList();

            var pedidosPagadosDelivery = await _context.PagosDelivery
                .SelectMany(x => x.Detalles)
                .Select(d => d.PedidoId)
                .ToListAsync();

            var deliveries = ventas
                .Where(v =>
                    v.Pedido != null &&
                    v.Pedido.TipoPedido == TipoPedido.Delivery &&
                    v.Pedido.DeliveryId.HasValue &&
                    v.Pedido.Delivery != null &&
                    v.TotalEnvio > 0 &&
                    v.Pagos.Any(p => p.FormaPago != FormaPago.Efectivo) &&
                    !pedidosPagadosDelivery.Contains(v.Pedido.Id))
                 .GroupBy(v => new
     {
         DeliveryId = v.Pedido!.DeliveryId!.Value,
         Nombre = v.Pedido.Delivery!.Nombre
     })
     .Select(g => new DeliveryInformeDto
     {
         DeliveryId = g.Key.DeliveryId,
         Nombre = g.Key.Nombre,
         CantidadEnvios = g.Count(),
         TotalDelivery = g.Sum(x => x.TotalEnvio),

         Envios = g
             .OrderByDescending(x => x.FechaVenta)
             .Select(x => new DeliveryEnvioDetalleDto
             {
                 PedidoId = x.Pedido!.Id,
                 Fecha = x.FechaVenta,
                 Cliente = x.Pedido.NombreCliente,
                 Direccion = x.Pedido.Direccion ?? "-",
                 TotalPedido = x.TotalGeneral,
                 Envio = x.TotalEnvio,
                 FormaPago = string.Join(", ",
                     x.Pagos.Select(p => ObtenerNombreFormaPago(p.FormaPago))),
                 Productos = x.Pedido.Items
                     .Select(i => $"{i.Cantidad}x {i.NombreProducto}")
                     .ToList()
             })
             .ToList()
     })
     .OrderByDescending(x => x.TotalDelivery)
     .Take(5)
     .ToList();

            return new InformeDashboardDto
            {
                VentasTotales = totalVentas,
                CantidadVentas = cantidadVentas,
                PedidosDelivery = pedidosDelivery,
                PedidosRetiro = pedidosRetiro,
                PromedioPorVenta = promedio,
                FormasPago = formasPago,
                ProductosMasVendidos = productosMasVendidos,
                Deliveries = deliveries
            };
        }

        private static string ObtenerNombreFormaPago(FormaPago formaPago)
        {
            return formaPago switch
            {
                FormaPago.Efectivo => "💵 Efectivo",
                FormaPago.Transferencia => "🏦 Transferencia",
                FormaPago.MercadoPago => "🟦 Mercado Pago",
                FormaPago.Debito => "💳 Débito",
                FormaPago.Credito => "💳 Crédito",
                FormaPago.Otro => "📌 Otro",
                _ => formaPago.ToString()
            };
        }
    }
}