using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace JarredsOrderHub.Models
{
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        [ForeignKey("Platillo")]
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }

        public virtual Pedidos? Pedido { get; set; }
        public virtual Platillo? Platillo { get; set; }
    }

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

        [ForeignKey("UsuarioId")]
        public virtual Cliente? Cliente { get; set; }
        public virtual List<DetallePedido>? Detalles { get; set; }

        [ForeignKey("Cupon")]
        public int? CuponId { get; set; } // Puede ser null si el pedido no tiene cupón
        public virtual Cupon? Cupon { get; set; }

    }
}
