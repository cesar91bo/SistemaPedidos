using SistemaPedidos.Application.Features.Categorias.DTOs;

namespace SistemaPedidos.Application.Features.Categorias.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ObtenerTodasAsync();
    Task<List<CategoriaDto>> ObtenerSoloActivosAsync();
    Task<CategoriaDto?> ObtenerPorIdAsync(int id);

    Task<int> CrearAsync(CrearCategoriaDto dto);

    Task ActualizarAsync(ActualizarCategoriaDto dto);

    Task DesactivarAsync(int id);
}
