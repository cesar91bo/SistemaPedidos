namespace SistemaPedidos.Application.Features.Productos.DTOs;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; } = string.Empty;
    public decimal PrecioActual { get; set; }
    public string? ImagenUrl { get; set; }
}
