using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Application.Features.Ventas.DTOs;

public class VentaDto
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public decimal TotalProductos { get; set; }

    public decimal TotalEnvio { get; set; }

    public decimal TotalGeneral { get; set; }

    public EstadoPago EstadoPago { get; set; }

    public bool CajaImpactada { get; set; }

    public List<PagoDto> Pagos { get; set; } = new();
}