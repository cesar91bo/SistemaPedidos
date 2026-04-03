using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Parametros.DTOs;
using SistemaPedidos.Application.Features.Parametros.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Domain.Enums.SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;

namespace SistemaPedidos.Infrastructure.Services.Parametros
{
    public class ParametroService : IParametroService
    {
        private readonly AppDbContext _context;

        public ParametroService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ParametroDto> CrearAsync(CrearParametroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Codigo))
                throw new ArgumentException("El código es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Valor))
                throw new ArgumentException("El valor es obligatorio.");

            dto.Codigo = dto.Codigo.Trim().ToUpper();

            var existe = await _context.Parametros
                .AnyAsync(x => x.Codigo == dto.Codigo);

            if (existe)
                throw new InvalidOperationException("Ya existe un parámetro con ese código.");

            ValidarValorSegunTipo(dto.Valor, dto.Tipo);

            var entidad = new Parametro
            {
                Codigo = dto.Codigo,
                Valor = dto.Valor.Trim(),
                Grupo = dto.Grupo,
                Descripcion = dto.Descripcion,
                Tipo = dto.Tipo,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _context.Parametros.Add(entidad);
            await _context.SaveChangesAsync();

            return MapearDto(entidad);
        }

        public async Task<IEnumerable<ParametroDto>> ObtenerTodosAsync()
        {
            var lista = await _context.Parametros
                .OrderBy(x => x.Codigo)
                .ToListAsync();

            return lista.Select(MapearDto);
        }

        public async Task<ParametroDto?> ObtenerPorIdAsync(int id)
        {
            var entidad = await _context.Parametros
                .FirstOrDefaultAsync(x => x.Id == id);

            return entidad == null ? null : MapearDto(entidad);
        }

        public async Task<ParametroDto?> ObtenerPorCodigoAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return null;

            codigo = codigo.Trim().ToUpper();

            var entidad = await _context.Parametros
                .FirstOrDefaultAsync(x => x.Codigo == codigo && x.Activo);

            return entidad == null ? null : MapearDto(entidad);
        }

        public async Task ActualizarAsync(int id, ActualizarParametroDto dto)
        {
            var entidad = await _context.Parametros
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
                throw new InvalidOperationException("No se encontró el parámetro.");

            if (string.IsNullOrWhiteSpace(dto.Valor))
                throw new ArgumentException("El valor es obligatorio.");

            ValidarValorSegunTipo(dto.Valor, entidad.Tipo);

            entidad.Valor = dto.Valor.Trim();
            entidad.Grupo = dto.Grupo;
            entidad.Descripcion = dto.Descripcion;
            entidad.FechaModificacion = DateTime.Now;

            if (dto.Activo.HasValue)
                entidad.Activo = dto.Activo.Value;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await _context.Parametros
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
                throw new InvalidOperationException("No se encontró el parámetro.");

            entidad.Activo = false;
            entidad.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        private static void ValidarValorSegunTipo(string valor, TipoParametro tipo)
        {
            switch (tipo)
            {
                case TipoParametro.Texto:
                    break;

                case TipoParametro.Entero:
                    if (!int.TryParse(valor, out _))
                        throw new ArgumentException("El valor no es un entero válido.");
                    break;

                case TipoParametro.Decimal:
                    if (!decimal.TryParse(valor, out _))
                        throw new ArgumentException("El valor no es un decimal válido.");
                    break;

                case TipoParametro.Booleano:
                    if (!bool.TryParse(valor, out _))
                        throw new ArgumentException("El valor no es un booleano válido.");
                    break;

                case TipoParametro.Fecha:
                    if (!DateTime.TryParse(valor, out _))
                        throw new ArgumentException("El valor no es una fecha válida.");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de parámetro no válido.");
            }
        }

        private static ParametroDto MapearDto(Parametro entidad)
        {
            return new ParametroDto
            {
                Id = entidad.Id,
                Codigo = entidad.Codigo,
                Valor = entidad.Valor,
                Grupo = entidad.Grupo,
                Descripcion = entidad.Descripcion,
                Tipo = entidad.Tipo,
                FechaCreacion = entidad.FechaCreacion,
                FechaModificacion = entidad.FechaModificacion,
                Activo = entidad.Activo
            };
        }
    }
}