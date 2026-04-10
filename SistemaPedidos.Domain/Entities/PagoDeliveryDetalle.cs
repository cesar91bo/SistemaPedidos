namespace SistemaPedidos.Domain.Entities
{
    public class PagoDeliveryDetalle
    {
        public int Id { get; set; }

        public int PagoDeliveryId { get; set; }
        public PagoDelivery PagoDelivery { get; set; } = null!;

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public decimal MontoEnvio { get; set; }
    }
}