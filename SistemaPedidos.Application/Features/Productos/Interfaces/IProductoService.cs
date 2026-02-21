using SistemaPedidos.Application.Features.Productos.DTOs;

namespace SistemaPedidos.Application.Features.Productos.Interfaces;

public interface IProductoService
{
    Task<List<ProductoDto>> ObtenerTodosAsync();

    Task<ProductoDto?> ObtenerPorIdAsync(int id);

    Task CrearAsync(CrearProductoDto dto);

    Task ActualizarAsync(ActualizarProductoDto dto);

    Task DesactivarAsync(int id);
}
