using SistemaPedidos.Application.Features.Parametros.DTOs;

namespace SistemaPedidos.Application.Features.Parametros.Interfaces
{
    public interface IParametroService
    {
        Task<ParametroDto> CrearAsync(CrearParametroDto dto);
        Task<IEnumerable<ParametroDto>> ObtenerTodosAsync();
        Task<ParametroDto?> ObtenerPorIdAsync(int id);
        Task<ParametroDto?> ObtenerPorCodigoAsync(string codigo);
        Task ActualizarAsync(int id, ActualizarParametroDto dto);
        Task EliminarAsync(int id);
    }
}