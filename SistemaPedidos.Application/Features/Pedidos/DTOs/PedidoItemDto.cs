namespace SistemaPedidos.Application.Features.Pedidos.DTOs;

public class PedidoItemDto
{
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string Observacion { get; set; } = string.Empty;
    public string? CategoriaNombre { get; set; }
}
