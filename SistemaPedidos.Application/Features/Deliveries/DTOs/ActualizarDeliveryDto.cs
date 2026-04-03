using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.DTOs
{
    public class ActualizarDeliveryDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
