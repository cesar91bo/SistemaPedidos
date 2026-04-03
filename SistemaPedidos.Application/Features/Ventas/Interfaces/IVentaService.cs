using SistemaPedidos.Application.Features.Ventas.DTOs;
using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Application.Features.Ventas;

public interface IVentaService
{
    Task RegistrarVentaAsync(int pedidoId, List<RegistrarPagoDto> pagos);
    Task<decimal> ObtenerTotalVentasDelDiaAsync();
    Task<decimal> ObtenerTotalVentasDeAyerAsync();
    Task<int> ObtenerCantidadVentasDelDiaAsync();
    Task<List<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(int cantidad = 4);
}