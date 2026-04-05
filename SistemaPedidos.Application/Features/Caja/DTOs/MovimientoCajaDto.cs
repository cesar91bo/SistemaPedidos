using SistemaPedidos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Caja.DTOs
{
    public class MovimientoCajaDto
    {
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public TipoMovimientoCaja Tipo { get; set; }
        public decimal Monto { get; set; }
    }
}
