using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Platillo
    {
        [Key]
        public int IdPlatillo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public string? Imagen { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        [Required]
        public bool Activo { get; set; }

        // PropiedadForeign
        [ForeignKey("IdCategoria")]
        public virtual Categoria? Categoria { get; set; }
    }
}