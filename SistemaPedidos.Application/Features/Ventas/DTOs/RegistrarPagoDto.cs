using SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Application.Features.Ventas.DTOs;

public class RegistrarPagoDto
{
    public int PedidoId { get; set; }

    public decimal Monto { get; set; }

    public FormaPago FormaPago { get; set; }
}