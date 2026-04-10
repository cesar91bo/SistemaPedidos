using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.DTOs
{
    public class DeliveryPendienteDto
    {
        public int DeliveryId { get; set; }
        public string NombreDelivery { get; set; } = string.Empty;
        public decimal TotalPendiente { get; set; }
        public List<DeliveryPendientePedidoDto> Pedidos { get; set; } = new();
    }
}
