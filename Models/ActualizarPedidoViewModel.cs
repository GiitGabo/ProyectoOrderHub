using System;
using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class ActualizarPedidoViewModel
    {
        [Required]
        public int IdPedido { get; set; }

        [Required]
        public string EstadoPedido { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public double? LatitudEntregaReal { get; set; }

        public double? LongitudEntregaReal { get; set; }
    }
}