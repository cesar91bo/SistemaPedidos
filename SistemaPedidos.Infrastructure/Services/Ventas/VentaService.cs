using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Caja.Interfaces;
using SistemaPedidos.Application.Features.Ventas;
using SistemaPedidos.Application.Features.Ventas.DTOs;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Infrastructure.Services.Ventas
{
    public class VentaService : IVentaService
    {
        private readonly AppDbContext _context;
        private readonly ICajaService _cajaService;

        public VentaService(AppDbContext context, ICajaService cajaService)
        {
            _context = context;
            _cajaService = cajaService;
        }

        public async Task RegistrarVentaAsync(int pedidoId, List<RegistrarPagoDto> pagos)
        {
            if (!await _cajaService.HayCajaAbiertaAsync())
                throw new Exception("Debe abrir una caja antes de registrar una venta.");

            var pedido = await _context.Pedidos
                .Include(p => p.Items)
                .Include(p => p.Delivery)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new Exception("Pedido no encontrado.");

            if (pedido.Estado == EstadoPedido.Terminado)
                throw new Exception("El pedido ya fue cobrado.");

            var totalPagos = pagos.Sum(x => x.Monto);

            if (totalPagos != pedido.TotalGeneral)
                throw new Exception("La suma de los pagos no coincide con el total del pedido.");

            var pagoTodoAlLocal = pagos.All(x => x.FormaPago != FormaPago.Efectivo);

            var montoDelivery = 0m;

            if (pedido.TipoPedido == TipoPedido.Delivery
                && pedido.DeliveryId.HasValue
                && pagoTodoAlLocal)
            {
                montoDelivery = pedido.PrecioEnvio;
            }

            var venta = new Venta
            {
                PedidoId = pedido.Id,
                FechaVenta = DateTime.Now,
                TotalProductos = pedido.TotalProductos,
                TotalEnvio = pedido.PrecioEnvio,
                TotalGeneral = pedido.TotalGeneral,
                MontoDelivery = montoDelivery,
                Anulada = false,
                Pagos = pagos.Select(x => new Pago
                {
                    FormaPago = x.FormaPago,
                    Monto = x.Monto,
                    Fecha = DateTime.Now
                }).ToList()
            };

            _context.Ventas.Add(venta);

            // Lo que realmente ingresa a caja para el local
            decimal ingresoCaja = 0;

            foreach (var pago in pagos)
            {
                if (pago.FormaPago == FormaPago.Efectivo)
                {
                    ingresoCaja += pago.Monto;

                    // Si el delivery cobra en efectivo, el envío no queda para el local
                    if (pedido.TipoPedido == TipoPedido.Delivery)
                    {
                        ingresoCaja -= pedido.PrecioEnvio;
                    }
                }
                else
                {
                    ingresoCaja += pago.Monto;
                }
            }

            // Si el cliente transfirió también el envío, ese importe luego va para el delivery
            ingresoCaja -= montoDelivery;

            if (ingresoCaja < 0)
                ingresoCaja = 0;

            if (ingresoCaja > 0)
            {
                await _cajaService.RegistrarIngresoAsync(
                    ingresoCaja,
                    pedido.TipoPedido == TipoPedido.Delivery && montoDelivery > 0
                        ? $"Cobro pedido #{pedido.Id} (sin envío de {pedido.Delivery?.Nombre})"
                        : $"Cobro pedido #{pedido.Id}",
                    pedido.Id
                );
            }

            pedido.Estado = EstadoPedido.Terminado;

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ObtenerTotalVentasDelDiaAsync()
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            return await _context.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < mañana)
                .SumAsync(v => (decimal?)(v.TotalGeneral - v.TotalEnvio)) ?? 0;
        }

        public async Task<decimal> ObtenerTotalVentasDeAyerAsync()
        {
            var ayer = DateTime.Today.AddDays(-1);
            var hoy = DateTime.Today;

            return await _context.Ventas
                .Where(v => v.FechaVenta >= ayer && v.FechaVenta < hoy)
                .SumAsync(v => (decimal?)(v.TotalGeneral - v.TotalEnvio)) ?? 0;
        }

        public async Task<int> ObtenerCantidadVentasDelDiaAsync()
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            return await _context.Ventas
                .CountAsync(v => v.FechaVenta >= hoy && v.FechaVenta < mañana);
        }

        public async Task<List<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(int cantidad = 4)
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            return await _context.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < mañana)
                .SelectMany(v => v.Pedido!.Items)
                .GroupBy(i => i.Producto!.Nombre)
                .Select(g => new ProductoMasVendidoDto
                {
                    Nombre = g.Key,
                    CantidadVendida = g.Sum(x => x.Cantidad)
                })
                .OrderByDescending(x => x.CantidadVendida)
                .Take(cantidad)
                .ToListAsync();
        }
    }
}
