using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    public decimal Total { get; set; }  // Snapshot histórico

    public List<PedidoItem> Items { get; set; } = new();
}
