namespace SistemaPedidos.Application.Features.Pedidos.DTOs;

public class PedidoDto
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string NombreCliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }

    public int TipoPedido { get; set; }
    public string TipoPedidoNombre { get; set; } = string.Empty;

    public int Estado { get; set; }
    public string EstadoNombre { get; set; } = string.Empty;

    public decimal TotalProductos { get; set; }
    public decimal PrecioEnvio { get; set; }
    public decimal TotalGeneral { get; set; }
    public int? DeliveryId { get; set; }
    public string? DeliveryNombre { get; set; }


    public List<PedidoItemDto> Items { get; set; } = new();
}
