using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Categorias.DTOs;
using SistemaPedidos.Application.Features.Categorias.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Categorias;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> ObtenerTodasAsync()
    {
        return await _context.Categorias
            .Where(c => c.Activo)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                ImagenUrl = c.ImagenUrl
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

        if (categoria == null)
            return null;

        return new CategoriaDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            ImagenUrl = categoria.ImagenUrl
        };
    }

    public async Task<int> CrearAsync(CrearCategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            ImagenUrl = dto.ImagenUrl,
            Activo = true
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return categoria.Id;
    }

    public async Task ActualizarAsync(ActualizarCategoriaDto dto)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == dto.Id && c.Activo);

        if (categoria == null)
            throw new Exception("Categoría no encontrada");

        categoria.Nombre = dto.Nombre;
        categoria.ImagenUrl = dto.ImagenUrl;

        await _context.SaveChangesAsync();
    }

    public async Task DesactivarAsync(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

        if (categoria == null)
            throw new Exception("Categoría no encontrada");

        categoria.Activo = false;

        await _context.SaveChangesAsync();
    }
}
