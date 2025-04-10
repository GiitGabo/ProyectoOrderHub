using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class CuponCliente
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cupon")]
        public int CuponId { get; set; }

        [ForeignKey("Usuario")]
        public int ClienteId { get; set; }

        public DateTime FechaUso { get; set; }

        public virtual Cupon Cupon { get; set; }
    }
}
