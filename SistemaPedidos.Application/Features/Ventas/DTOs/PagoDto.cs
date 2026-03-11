using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Application.Features.Ventas.DTOs;

public class PagoDto
{
    public decimal Monto { get; set; }

    public FormaPago FormaPago { get; set; }

    public DateTime Fecha { get; set; }
}