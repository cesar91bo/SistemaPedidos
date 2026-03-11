using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Application.Features.Ventas.DTOs;

namespace SistemaPedidos.Application.Features.Ventas;

public interface IVentaService
{
    Task RegistrarVentaAsync(int pedidoId, List<RegistrarPagoDto> pagos);
}