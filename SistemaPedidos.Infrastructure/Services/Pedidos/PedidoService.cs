using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Pedidos.DTOs;
using SistemaPedidos.Application.Features.Pedidos.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Pedidos;

public class PedidoService : IPedidoService
{
    private readonly AppDbContext _context;

    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PedidoDto>> ObtenerActivosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
            .Include(p => p.Delivery)
            .Where(p => p.Estado != EstadoPedido.Terminado &&
                        p.Estado != EstadoPedido.Cancelado)
            .OrderByDescending(p => p.Fecha)
            .Select(p => new PedidoDto
            {
                Id = p.Id,
                Fecha = p.Fecha,
                NombreCliente = p.NombreCliente,
                Telefono = p.Telefono,
                Direccion = p.Direccion,
                TipoPedido = (int)p.TipoPedido,
                TipoPedidoNombre = p.TipoPedido.ToString(),
                Estado = (int)p.Estado,
                EstadoNombre =
                    p.Estado == EstadoPedido.EnPreparacion ? "En preparación" :
                    p.Estado == EstadoPedido.Listo ? "Listo" :
                    p.Estado == EstadoPedido.EnCamino ? "En camino" :
                    p.Estado == EstadoPedido.Terminado ? "Terminado" :
                    p.Estado == EstadoPedido.Cancelado ? "Cancelado" :
                    p.Estado.ToString(),
                TotalProductos = p.TotalProductos,
                PrecioEnvio = p.PrecioEnvio,
                TotalGeneral = p.TotalGeneral,
                DeliveryId = p.DeliveryId,
                DeliveryNombre = p.Delivery != null ? p.Delivery.Nombre : null,
                Items = p.Items.Select(i => new PedidoItemDto
                {
                    ProductoId = i.ProductoId,
                    ProductoNombre = i.Producto!.Nombre,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<PedidoDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
            .Include(p => p.Delivery)
            .Where(p => p.Id == id)
            .Select(p => new PedidoDto
            {
                Id = p.Id,
                Fecha = p.Fecha,
                NombreCliente = p.NombreCliente,
                Telefono = p.Telefono,
                Direccion = p.Direccion,
                TipoPedido = (int)p.TipoPedido,
                TipoPedidoNombre = p.TipoPedido.ToString(),
                Estado = (int)p.Estado,
                EstadoNombre =
                    p.Estado == EstadoPedido.EnPreparacion ? "En preparación" :
                    p.Estado == EstadoPedido.Listo ? "Listo" :
                    p.Estado == EstadoPedido.EnCamino ? "En camino" :
                    p.Estado == EstadoPedido.Terminado ? "Terminado" :
                    p.Estado == EstadoPedido.Cancelado ? "Cancelado" :
                    p.Estado.ToString(),
                TotalProductos = p.TotalProductos,
                PrecioEnvio = p.PrecioEnvio,
                TotalGeneral = p.TotalGeneral,
                DeliveryId = p.DeliveryId,
                DeliveryNombre = p.Delivery != null ? p.Delivery.Nombre : null,
                Items = p.Items.Select(i => new PedidoItemDto
                {
                    ProductoId = i.ProductoId,
                    ProductoNombre = i.Producto!.Nombre,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    Subtotal = i.Subtotal
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CrearAsync(CrearPedidoDto dto)
    {
        var pedido = new Pedido
        {
            Fecha = DateTime.Now,
            NombreCliente = dto.NombreCliente,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            TipoPedido = (TipoPedido)dto.TipoPedido,
            Estado = EstadoPedido.EnPreparacion,
            PrecioEnvio = dto.PrecioEnvio,
            DeliveryId = dto.DeliveryId
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        decimal totalProductos = 0;

        foreach (var item in dto.Items)
        {
            var precioActual = await _context.PreciosProducto
                .Where(p => p.ProductoId == item.ProductoId && p.FechaHasta == null)
                .Select(p => p.Importe)
                .FirstOrDefaultAsync();

            var subtotal = precioActual * item.Cantidad;

            var pedidoItem = new PedidoItem
            {
                PedidoId = pedido.Id,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = precioActual,
                Subtotal = subtotal
            };

            totalProductos += subtotal;

            _context.PedidoItems.Add(pedidoItem);
        }

        pedido.TotalProductos = totalProductos;
        pedido.TotalGeneral = totalProductos + pedido.PrecioEnvio;

        await _context.SaveChangesAsync();

        return pedido.Id;
    }

    public async Task CambiarEstadoAsync(int pedidoId, int nuevoEstado)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);

        if (pedido == null)
            return;

        pedido.Estado = (EstadoPedido)nuevoEstado;

        await _context.SaveChangesAsync();
    }
}


