using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum EstadoPedido
{
    Pendiente = 1,
    Confirmado = 2,
    Cancelado = 3,
    Entregado = 4
}