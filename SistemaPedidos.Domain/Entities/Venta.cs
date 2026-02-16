using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities;

public class Venta
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.Now;

    public decimal TotalVenta { get; set; } // Snapshot

    public EstadoPago EstadoPago { get; set; } = EstadoPago.Pendiente;

    public List<Pago> Pagos { get; set; } = new();
}
