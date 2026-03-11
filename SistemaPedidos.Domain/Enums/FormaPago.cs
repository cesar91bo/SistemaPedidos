using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Enums;

public enum FormaPago
{
    Efectivo = 1,
    Transferencia = 2,
    [Display(Name = "Mercado Pago")]
    MercadoPago = 3,
    Debito = 4,
    Credito = 5,
    Otro = 6
}

