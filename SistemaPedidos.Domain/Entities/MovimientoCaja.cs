using SistemaPedidos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Entities
{
    public class MovimientoCaja
    {
        public int Id { get; set; }

        public int CajaId { get; set; }

        public TipoMovimientoCaja Tipo { get; set; }

        public string? Concepto { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }
    }
}
