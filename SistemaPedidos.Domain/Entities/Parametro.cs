using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Enums.SistemaPedidos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Domain.Entities
{
    public class Parametro
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Valor { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Grupo { get; set; }

        [MaxLength(250)]
        public string? Descripcion { get; set; }

        public TipoParametro Tipo { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; } = true;
    }
}