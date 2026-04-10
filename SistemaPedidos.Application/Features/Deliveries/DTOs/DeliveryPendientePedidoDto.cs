using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.DTOs
{
    public class DeliveryPendientePedidoDto
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal MontoEnvio { get; set; }
    }
}
