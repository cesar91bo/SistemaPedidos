using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Domain.Entities;

public class Pago
{
    public int Id { get; set; }
    public int VentaId { get; set; }

    public decimal Monto { get; set; }

    public FormaPago FormaPago { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;
}
