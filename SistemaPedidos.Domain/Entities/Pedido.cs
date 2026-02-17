using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public TipoPedido TipoPedido { get; set; }

    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    // Datos del cliente
    public string NombreCliente { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Direccion { get; set; } // Solo obligatorio si es Delivery

    // Valores monetarios
    public decimal PrecioEnvio { get; set; }

    public decimal TotalProductos { get; set; }

    public decimal TotalGeneral { get; set; }

    public List<PedidoItem> Items { get; set; } = new();
}
