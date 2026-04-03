using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Application.Features.Deliveries.DTOs;
using SistemaPedidos.Application.Features.Deliveries.Interfaces;
using SistemaPedidos.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Infrastructure.Services.Deliveries
{
    public class DeliveryService : IDeliveryService
    {
        private readonly AppDbContext _context;

        public DeliveryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryDto>> ObtenerSoloActivosAsync()
        {
            return await _context.Deliveries
                .Where(x => x.Activo)
                .OrderBy(x => x.Nombre)
                .Select(x => new DeliveryDto
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Telefono = x.Telefono
                })
                .ToListAsync();
        }

        public async Task CrearAsync(CrearDeliveryDto dto)
        {
            var delivery = new Delivery
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Activo = true
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(ActualizarDeliveryDto dto)
        {
            var delivery = await _context.Deliveries.FindAsync(dto.Id);

            if (delivery == null)
                return;

            delivery.Nombre = dto.Nombre;
            delivery.Telefono = dto.Telefono;

            await _context.SaveChangesAsync();
        }

        public async Task DesactivarAsync(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);

            if (delivery == null)
                return;

            delivery.Activo = false;

            await _context.SaveChangesAsync();
        }
    }
}
