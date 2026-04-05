using SistemaPedidos.Application.Features.Pedidos.DTOs;

namespace SistemaPedidos.Application.Features.Pedidos.Interfaces;

public interface IPedidoService
{
    Task<List<PedidoDto>> ObtenerActivosAsync();

    Task<PedidoDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearPedidoDto dto);

    Task CambiarEstadoAsync(int pedidoId, int nuevoEstado);

    Task AsignarDeliveryAsync(int pedidoId, int deliveryId);

}
