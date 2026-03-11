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
    }
}
