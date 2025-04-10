﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace JarredsOrderHub.Models
{
    public class Pedidos
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public string EstadoPedido { get; set; } = "Pendiente"; // Estados: Pendiente, En preparación, En camino, Entregado, Cancelado
        public string MetodoPago { get; set; } // Efectivo, Tarjeta, PayPal
        public decimal Total { get; set; }
        public string Comentarios { get; set; }
        [ForeignKey("Cupon")]
        public int? CuponId { get; set; } // Puede ser null si el pedido no tiene cupón
        public virtual Cupon? Cupon { get; set; }

        public virtual List<DetallePedido>? Detalles { get; set; }
    }
}
