namespace SistemaPedidos.Application.Features.Categorias.DTOs;

public class ActualizarCategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }

}
