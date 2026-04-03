using SistemaPedidos.Application.Features.Deliveries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Deliveries.Interfaces
{
    public interface IDeliveryService
    {
        Task<List<DeliveryDto>> ObtenerSoloActivosAsync();
        Task CrearAsync(CrearDeliveryDto dto);
        Task ActualizarAsync(ActualizarDeliveryDto dto);
        Task DesactivarAsync(int id);
    }
}
