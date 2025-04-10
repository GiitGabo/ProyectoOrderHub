using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class Cupon
    {
        [Key]
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public decimal Descuento { get; set; } // Monto de descuento (puede ser en porcentaje o cantidad fija)
        public DateTime FechaExpiracion { get; set; }
        public bool EsPorcentual { get; set; }
    }
}
