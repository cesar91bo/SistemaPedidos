namespace SistemaPedidos.Application.Features.Pedidos.DTOs;

public class CrearPedidoItemDto
{
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}
