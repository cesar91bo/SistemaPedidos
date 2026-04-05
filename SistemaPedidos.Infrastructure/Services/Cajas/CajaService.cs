using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Caja.DTOs;
using SistemaPedidos.Application.Features.Caja.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Enums;
using SistemaPedidos.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Infrastructure.Services.Cajas
{
    public class CajaService : ICajaService
    {
        private readonly AppDbContext _context;

        public CajaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CajaDto?> ObtenerCajaAbiertaAsync()
        {
            var caja = await _context.Cajas
                .Include(x => x.Movimientos)
                .FirstOrDefaultAsync(x => x.Estado == EstadoCaja.Abierta);

            if (caja == null)
                return null;

            var ingresos = caja.Movimientos
                .Where(x => x.Tipo == TipoMovimientoCaja.Ingreso || x.Tipo == TipoMovimientoCaja.Apertura)
                .Sum(x => x.Monto);

            var retiros = caja.Movimientos
                .Where(x => x.Tipo == TipoMovimientoCaja.Retiro)
                .Sum(x => x.Monto);

            return new CajaDto
            {
                Id = caja.Id,
                FechaApertura = caja.FechaApertura,
                FechaCierre = caja.FechaCierre,
                FondoInicial = caja.FondoInicial,
                TotalIngresos = ingresos,
                TotalRetiros = retiros,
                Disponible = ingresos - retiros,
                Estado = caja.Estado,
                Movimientos = caja.Movimientos
                    .OrderByDescending(x => x.Fecha)
                    .Select(x => new MovimientoCajaDto
                    {
                        Fecha = x.Fecha,
                        Descripcion = x.Descripcion,
                        Tipo = x.Tipo,
                        Monto = x.Monto
                    })
                    .ToList()
            };
        }

        public async Task<int> AbrirAsync(AbrirCajaDto dto)
        {
            var existeCajaAbierta = await _context.Cajas
                .AnyAsync(x => x.Estado == EstadoCaja.Abierta);

            if (existeCajaAbierta)
                throw new Exception("Ya existe una caja abierta.");

            var caja = new Caja
            {
                FechaApertura = DateTime.Now,
                FondoInicial = dto.FondoInicial,
                Estado = EstadoCaja.Abierta
            };

            _context.Cajas.Add(caja);
            await _context.SaveChangesAsync();

            _context.MovimientosCaja.Add(new MovimientoCaja
            {
                CajaId = caja.Id,
                Fecha = DateTime.Now,
                Tipo = TipoMovimientoCaja.Apertura,
                Monto = dto.FondoInicial,
                Descripcion = "Apertura de caja"
            });

            await _context.SaveChangesAsync();

            return caja.Id;
        }

        public async Task RegistrarIngresoAsync(decimal monto, string descripcion, int? pedidoId = null)
        {
            var caja = await _context.Cajas
                .FirstOrDefaultAsync(x => x.Estado == EstadoCaja.Abierta);

            if (caja == null)
                throw new Exception("No hay una caja abierta.");

            _context.MovimientosCaja.Add(new MovimientoCaja
            {
                CajaId = caja.Id,
                Fecha = DateTime.Now,
                Tipo = TipoMovimientoCaja.Ingreso,
                Monto = monto,
                Descripcion = descripcion,
                PedidoId = pedidoId
            });

            await _context.SaveChangesAsync();
        }

        public async Task RegistrarRetiroAsync(RetiroCajaDto dto)
        {
            var caja = await _context.Cajas
                .FirstOrDefaultAsync(x => x.Estado == EstadoCaja.Abierta);

            if (caja == null)
                throw new Exception("No hay una caja abierta.");

            var ingresos = await _context.MovimientosCaja
                .Where(x => x.CajaId == caja.Id &&
                       (x.Tipo == TipoMovimientoCaja.Ingreso || x.Tipo == TipoMovimientoCaja.Apertura))
                .SumAsync(x => x.Monto);

            var retiros = await _context.MovimientosCaja
                .Where(x => x.CajaId == caja.Id &&
                       x.Tipo == TipoMovimientoCaja.Retiro)
                .SumAsync(x => x.Monto);

            var disponible = ingresos - retiros;

            if (dto.Monto > disponible)
                throw new Exception("El monto supera el dinero disponible en caja.");

            _context.MovimientosCaja.Add(new MovimientoCaja
            {
                CajaId = caja.Id,
                Fecha = DateTime.Now,
                Tipo = TipoMovimientoCaja.Retiro,
                Monto = dto.Monto,
                Descripcion = dto.Descripcion
            });

            await _context.SaveChangesAsync();
        }

        public async Task CerrarAsync()
        {
            var caja = await _context.Cajas
                .Include(x => x.Movimientos)
                .FirstOrDefaultAsync(x => x.Estado == EstadoCaja.Abierta);

            if (caja == null)
                throw new Exception("No hay una caja abierta.");

            var ingresos = caja.Movimientos
                .Where(x => x.Tipo == TipoMovimientoCaja.Ingreso || x.Tipo == TipoMovimientoCaja.Apertura)
                .Sum(x => x.Monto);

            var retiros = caja.Movimientos
                .Where(x => x.Tipo == TipoMovimientoCaja.Retiro)
                .Sum(x => x.Monto);

            caja.TotalFinal = ingresos - retiros;
            caja.TotalEfectivo = ingresos;
            caja.TotalRetiros = retiros;
            caja.FechaCierre = DateTime.Now;
            caja.Estado = EstadoCaja.Cerrada;

            _context.MovimientosCaja.Add(new MovimientoCaja
            {
                CajaId = caja.Id,
                Fecha = DateTime.Now,
                Tipo = TipoMovimientoCaja.Cierre,
                Monto = caja.TotalFinal,
                Descripcion = "Cierre de caja"
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> HayCajaAbiertaAsync()
        {
            return await _context.Cajas
                .AnyAsync(c => c.Estado == EstadoCaja.Abierta);
        }
    }
}
