namespace SistemaPedidos.Application.Features.Productos.DTOs;

public class CrearProductoDto
{
    public string Nombre { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public decimal PrecioInicial { get; set; }
    public string? ImagenUrl { get; set; }
}
