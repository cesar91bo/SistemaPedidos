using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Enums.SistemaPedidos.Domain.Enums;

namespace SistemaPedidos.Application.Features.Parametros.DTOs
{
    public class CrearParametroDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string? Grupo { get; set; }
        public string? Descripcion { get; set; }
        public TipoParametro Tipo { get; set; }
    }
}