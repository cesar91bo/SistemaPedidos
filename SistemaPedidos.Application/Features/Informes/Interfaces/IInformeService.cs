using SistemaPedidos.Application.Features.Informes.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Informes.Interfaces
{
    public interface IInformeService
    {
        Task<InformeDashboardDto> ObtenerDashboardAsync(InformeFiltroDto filtro);
    }
}
