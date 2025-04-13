using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pedidos")]
        public int PedidoId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagado
        public DateTime? FechaPago { get; set; }

        public virtual Pedidos Pedido { get; set; }
    }
}
