using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPedidos.Application.Features.Informes.DTOs
{
    public class InformeDashboardDto
    {
        public decimal VentasTotales { get; set; }

        public int CantidadVentas { get; set; }

        public int PedidosDelivery { get; set; }

        public int PedidosRetiro { get; set; }

        public decimal PromedioPorVenta { get; set; }

        public List<FormaPagoInformeDto> FormasPago { get; set; } = new();

        public List<ProductoVendidoDto> ProductosMasVendidos { get; set; } = new();

        public List<DeliveryInformeDto> Deliveries { get; set; } = new();
    }

    public class FormaPagoInformeDto
    {
        public string Nombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal Total { get; set; }
    }

    public class ProductoVendidoDto
    {
        public string Nombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }
    }

    public class DeliveryInformeDto
    {
        public string Nombre { get; set; } = string.Empty;

        public int CantidadEnvios { get; set; }

        public decimal TotalDelivery { get; set; }

        public List<DeliveryEnvioDetalleDto> Envios { get; set; } = new();

        public int DeliveryId { get; set; }
    }

    public class DeliveryEnvioDetalleDto
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal TotalPedido { get; set; }
        public decimal Envio { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public List<string> Productos { get; set; } = new();
    }
}
