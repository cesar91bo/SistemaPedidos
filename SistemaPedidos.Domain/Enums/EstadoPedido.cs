using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum EstadoPedido
{
    EnProceso = 1,
    Enviado = 2,
    Terminado = 3,
    Cancelado = 4
}