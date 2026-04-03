using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Enums.SistemaPedidos.Domain.Enums;
using System;

namespace SistemaPedidos.Application.Features.Parametros.DTOs
{
    public class ParametroDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string? Grupo { get; set; }
        public string? Descripcion { get; set; }
        public TipoParametro Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}