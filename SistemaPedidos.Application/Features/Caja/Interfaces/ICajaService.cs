using SistemaPedidos.Application.Features.Caja.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Caja.Interfaces
{
    public interface ICajaService
    {
        Task<CajaDto?> ObtenerCajaAbiertaAsync();

        Task<int> AbrirAsync(AbrirCajaDto dto);

        Task RegistrarIngresoAsync(decimal monto, string descripcion, int? pedidoId = null);

        Task RegistrarRetiroAsync(RetiroCajaDto dto);

        Task CerrarAsync();
    }
}
