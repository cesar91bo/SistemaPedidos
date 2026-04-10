using SistemaPedidos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.DTOs
{
    public class RegistrarPagoDeliveryDto
    {
        public int DeliveryId { get; set; }
        public FormaPago FormaPago { get; set; }
        public string? Observacion { get; set; }

        // pedidos que se van a pagar
        public List<int> PedidoIds { get; set; } = new();
    }
}
