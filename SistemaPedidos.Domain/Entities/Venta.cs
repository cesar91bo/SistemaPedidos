using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities;

public class Venta
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public DateTime FechaVenta { get; set; }
    public decimal TotalProductos { get; set; }
    public decimal TotalEnvio { get; set; }
    public decimal TotalGeneral { get; set; }
    public decimal MontoDelivery { get; set; }
    public bool Anulada { get; set; }
    public List<Pago> Pagos { get; set; } = new();
    public Pedido Pedido { get; set; } = null!;
}