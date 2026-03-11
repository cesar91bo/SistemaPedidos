using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Entities
{
    public class Caja
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal MontoInicial { get; set; }

        public bool Abierta { get; set; }

        public DateTime FechaApertura { get; set; }

        public DateTime? FechaCierre { get; set; }

        public List<MovimientoCaja> Movimientos { get; set; } = new();
    }
}
