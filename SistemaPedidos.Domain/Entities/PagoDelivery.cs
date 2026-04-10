using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities
{
    public class PagoDelivery
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }
        public Delivery Delivery { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public FormaPago FormaPago { get; set; }

        public string? Observacion { get; set; }

        public List<PagoDeliveryDetalle> Detalles { get; set; } = new();
    }
}