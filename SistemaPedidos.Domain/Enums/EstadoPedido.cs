using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum EstadoPedido
{
    Pendiente = 1,
    EnProceso = 2,
    Listo = 3,
    Enviado = 4,
    Terminado = 5,
    Cancelado = 6
}