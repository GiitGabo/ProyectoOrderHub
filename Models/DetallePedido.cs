using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [ForeignKey("ProductoId")]
        public virtual Platillo? Platillo { get; set; }
    }
}
