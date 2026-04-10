using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Caja.DTOs;
using SistemaPedidos.Application.Features.Caja.Interfaces;
using SistemaPedidos.Application.Features.Deliveries.DTOs;
using SistemaPedidos.Application.Features.Deliveries.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Deliveries
{
    public class PagoDeliveryService : IPagoDeliveryService
    {
        private readonly AppDbContext _context;
        private readonly ICajaService _cajaService;

        public PagoDeliveryService(
            AppDbContext context,
            ICajaService cajaService)
        {
            _context = context;
            _cajaService = cajaService;
        }

        public async Task<List<DeliveryPendienteDto>> ObtenerPendientesAsync()
        {
            var pedidosPendientes = await _context.Pedidos
                .Include(p => p.Delivery)
                .Include(p => p.PagosDeliveryDetalle)
                .Include(p => p.Venta)
                    .ThenInclude(v => v.Pagos)
                .Where(p =>
                    p.DeliveryId != null &&
                    p.TipoPedido == TipoPedido.Delivery &&
                    p.PrecioEnvio > 0 &&
                    !p.PagosDeliveryDetalle.Any() &&
                    p.Venta != null &&
                    p.Venta.Pagos.Any(x => x.FormaPago != FormaPago.Efectivo))
                .ToListAsync();

            return pedidosPendientes
                .GroupBy(p => new
                {
                    p.DeliveryId,
                    Nombre = p.Delivery!.Nombre
                })
                .Select(g => new DeliveryPendienteDto
                {
                    DeliveryId = g.Key.DeliveryId!.Value,
                    NombreDelivery = g.Key.Nombre,
                    TotalPendiente = g.Sum(x => x.PrecioEnvio),
                    Pedidos = g.Select(x => new DeliveryPendientePedidoDto
                    {
                        PedidoId = x.Id,
                        Fecha = x.Fecha,
                        Cliente = x.NombreCliente,
                        Direccion = x.Direccion ?? "-",
                        MontoEnvio = x.PrecioEnvio
                    })
                .OrderByDescending(x => x.Fecha)
                .ToList()
                })
            .OrderByDescending(x => x.TotalPendiente)
            .ToList();
        }

        public async Task RegistrarPagoAsync(RegistrarPagoDeliveryDto dto)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Delivery)
                .Include(p => p.PagosDeliveryDetalle)
                .Where(p => dto.PedidoIds.Contains(p.Id))
                .ToListAsync();

            if (!pedidos.Any())
                throw new Exception("No se encontraron pedidos para pagar.");

            if (pedidos.Any(p => p.PagosDeliveryDetalle.Any()))
                throw new Exception("Uno o más pedidos ya fueron pagados al delivery.");

            var total = pedidos.Sum(p => p.PrecioEnvio);
            var pago = new PagoDelivery
            {
                DeliveryId = dto.DeliveryId,
                Fecha = DateTime.Now,
                FormaPago = dto.FormaPago,
                Monto = total,
                Observacion = dto.Observacion,
                Detalles = pedidos.Select(p => new PagoDeliveryDetalle
                {
                    PedidoId = p.Id,
                    MontoEnvio = p.PrecioEnvio
                }).ToList()
            };

            _context.PagosDelivery.Add(pago);

            // Si se paga en efectivo, sale dinero de caja
            if (dto.FormaPago == FormaPago.Efectivo)
            {
                if (!await _cajaService.HayCajaAbiertaAsync())
                    throw new Exception("Debe haber una caja abierta para pagar al delivery en efectivo.");

                var nombreDelivery = pedidos.First().Delivery?.Nombre ?? "Delivery";

                await _cajaService.RegistrarRetiroAsync(new RetiroCajaDto
                {
                    Monto = total,
                    Descripcion = $"Pago a delivery {nombreDelivery}"
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
