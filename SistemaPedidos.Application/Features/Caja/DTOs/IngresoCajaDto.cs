using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Caja.DTOs
{
    public class IngresoCajaDto
    {
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = "";
    }
}
