using Microsoft.EntityFrameworkCore;
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

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarVentaAsync(int pedidoId, List<RegistrarPagoDto> pagos)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new Exception("Pedido no encontrado.");

            var totalPagos = pagos.Sum(p => p.Monto);

            if (totalPagos != pedido.TotalGeneral)
                throw new Exception("La suma de los pagos no coincide con el total.");

            var venta = new Venta
            {
                PedidoId = pedido.Id,
                FechaVenta = DateTime.Now,
                TotalProductos = pedido.TotalProductos,
                TotalEnvio = pedido.PrecioEnvio,
                TotalGeneral = pedido.TotalGeneral,
                Anulada = false,
                Pagos = pagos.Select(p => new Pago
                {
                    Monto = p.Monto,
                    FormaPago = p.FormaPago,
                    Fecha = DateTime.Now
                }).ToList()
            };

            _context.Ventas.Add(venta);

            pedido.Estado = EstadoPedido.Terminado;

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ObtenerTotalVentasDelDiaAsync()
        {
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            return await _context.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < mañana)
                .SumAsync(v => (decimal?)v.TotalGeneral) ?? 0;
        }

        public async Task<decimal> ObtenerTotalVentasDeAyerAsync()
        {
            var ayer = DateTime.Today.AddDays(-1);
            var hoy = DateTime.Today;

            return await _context.Ventas
                .Where(v => v.FechaVenta >= ayer && v.FechaVenta < hoy)
                .SumAsync(v => (decimal?)v.TotalGeneral) ?? 0;
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
                //.Where(v => v.FechaVenta >= hoy && v.FechaVenta < mañana)
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
