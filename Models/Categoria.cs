using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Platillos = new List<Platillo>();
        }

        [Key]
        [Required]
        public int IdCategoria { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public bool Activa { get; set; }

        public virtual ICollection<Platillo> Platillos { get; set; }
    }
}
