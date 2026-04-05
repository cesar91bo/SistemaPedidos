namespace SistemaPedidos.Domain.Entities;

public class PedidoItem
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public string NombreProducto { get; set; } = string.Empty; // Snapshot

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; } // Snapshot
    public decimal Subtotal { get; set; }       // Snapshot

    public string Observacion { get; set; } = string.Empty;
}
