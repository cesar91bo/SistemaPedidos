using SistemaPedidos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Caja.DTOs
{
    public class CajaDto
    {
        public int Id { get; set; }

        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }

        public decimal FondoInicial { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalRetiros { get; set; }
        public decimal Disponible { get; set; }

        public EstadoCaja Estado { get; set; }

        public List<MovimientoCajaDto> Movimientos { get; set; } = new();
    }
}
