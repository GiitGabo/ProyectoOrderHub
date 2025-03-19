using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual List<DetallePedido>? Detalles { get; set; }
    }
}
