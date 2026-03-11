using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum EstadoPago
{
    Pendiente = 0,
    Parcial = 1,
    Pagado = 2,
    Anulada = 3
}

