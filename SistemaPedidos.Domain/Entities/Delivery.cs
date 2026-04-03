using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Delivery
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;
}
