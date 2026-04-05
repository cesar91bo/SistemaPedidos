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
        public Caja Caja { get; set; } = null!;

        public DateTime Fecha { get; set; }

        public TipoMovimientoCaja Tipo { get; set; }
        // Apertura, Ingreso, Retiro, Ajuste, Cierre

        public decimal Monto { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int? PedidoId { get; set; }

        public Pedido? Pedido { get; set; }
    }
}
