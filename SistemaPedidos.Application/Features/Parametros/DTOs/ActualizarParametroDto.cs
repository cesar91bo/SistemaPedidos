namespace SistemaPedidos.Application.Features.Parametros.DTOs
{
    public class ActualizarParametroDto
    {
        public string Valor { get; set; } = string.Empty;
        public string? Grupo { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}