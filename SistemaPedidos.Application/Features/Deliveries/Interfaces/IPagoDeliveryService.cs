using SistemaPedidos.Application.Features.Deliveries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.Interfaces
{
    public interface IPagoDeliveryService
    {
        Task<List<DeliveryPendienteDto>> ObtenerPendientesAsync();
        Task RegistrarPagoAsync(RegistrarPagoDeliveryDto dto);
    }
}
