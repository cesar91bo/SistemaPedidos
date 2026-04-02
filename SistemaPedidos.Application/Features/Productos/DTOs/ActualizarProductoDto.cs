namespace SistemaPedidos.Application.Features.Productos.DTOs;

public class ActualizarProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public decimal PrecioNuevo { get; set; }
    public string? ImagenUrl { get; set; }
}
