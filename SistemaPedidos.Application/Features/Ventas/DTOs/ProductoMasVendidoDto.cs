using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Ventas.DTOs
{
    public class ProductoMasVendidoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
    }
}
