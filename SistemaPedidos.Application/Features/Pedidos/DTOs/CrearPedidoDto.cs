namespace SistemaPedidos.Application.Features.Pedidos.DTOs;

public class CrearPedidoDto
{
    public string NombreCliente { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Direccion { get; set; }

    public int TipoPedido { get; set; }
    public decimal PrecioEnvio { get; set; }
    public int? DeliveryId { get; set; }

    public List<CrearPedidoItemDto> Items { get; set; } = new();
}
