using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum EstadoPedido
{
    EnPreparacion = 1,
    Listo = 2,
    EnCamino = 3,
    Terminado = 4,
    Cancelado = 5
}