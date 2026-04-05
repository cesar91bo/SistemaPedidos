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
        .           ThenInclude(p => p.Items)
                .Include(v => v.Pagos)
                .Where(v => v.FechaVenta >= fechaDesde &&
                v.FechaVenta < fechaHasta).ToListAsync();

            var totalVentas = ventas.Sum(v => v.TotalGeneral);
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
                .SelectMany(v => v.Pagos)
                .GroupBy(p => p.FormaPago)
                .Select(g => new FormaPagoInformeDto
                {
                    Nombre = ObtenerNombreFormaPago(g.Key),
                    Cantidad = g.Count(),
                    Total = g.Sum(x => x.Monto)
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

            var deliveries = await _context.Pedidos
                .Include(p => p.Delivery)
                .Where(p =>
                    p.DeliveryId != null &&
                    p.Fecha >= fechaDesde &&
                    p.Fecha < fechaHasta)
                .GroupBy(p => p.Delivery!.Nombre)
                .Select(g => new DeliveryInformeDto
                {
                    Nombre = g.Key,
                    CantidadEnvios = g.Count()
                })
                .OrderByDescending(x => x.CantidadEnvios)
                .Take(5)
                .ToListAsync();

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