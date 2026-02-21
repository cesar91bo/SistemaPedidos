using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Productos.DTOs;
using SistemaPedidos.Application.Features.Productos.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        return await _context.Productos
            .Where(p => p.Activo)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria!.Nombre,
                PrecioActual = p.Precios
                    .Where(pr => pr.FechaHasta == null)
                    .Select(pr => pr.Importe)
                    .FirstOrDefault(),
                ImagenUrl = p.ImagenUrl
            })
            .ToListAsync();
    }

    public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos
            .Where(p => p.Id == id && p.Activo)
            .Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria!.Nombre,
                PrecioActual = p.Precios
                    .Where(pr => pr.FechaHasta == null)
                    .Select(pr => pr.Importe)
                    .FirstOrDefault(),
                ImagenUrl = p.ImagenUrl
            })
            .FirstOrDefaultAsync();
    }

    public async Task CrearAsync(CrearProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            CategoriaId = dto.CategoriaId,
            ImagenUrl = dto.ImagenUrl,
            Activo = true
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        var precio = new PrecioProducto
        {
            ProductoId = producto.Id,
            Importe = dto.PrecioInicial,
            FechaDesde = DateTime.Now
        };

        _context.PreciosProducto.Add(precio);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(ActualizarProductoDto dto)
    {
        var producto = await _context.Productos
            .Include(p => p.Precios)
            .FirstOrDefaultAsync(p => p.Id == dto.Id);

        if (producto == null)
            return;

        producto.Nombre = dto.Nombre;
        producto.CategoriaId = dto.CategoriaId;
        producto.ImagenUrl = dto.ImagenUrl;

        var precioActual = producto.Precios
            .FirstOrDefault(p => p.FechaHasta == null);

        if (precioActual != null && precioActual.Importe != dto.PrecioNuevo)
        {
            precioActual.FechaHasta = DateTime.Now;

            var nuevoPrecio = new PrecioProducto
            {
                ProductoId = producto.Id,
                Importe = dto.PrecioNuevo,
                FechaDesde = DateTime.Now
            };

            _context.PreciosProducto.Add(nuevoPrecio);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return;

        producto.Activo = false;

        await _context.SaveChangesAsync();
    }
}
