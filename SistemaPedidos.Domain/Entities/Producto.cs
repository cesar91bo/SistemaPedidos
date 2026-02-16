using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public List<PrecioProducto> Precios { get; set; } = new();
    }
}
