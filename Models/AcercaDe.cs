using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class AcercaDe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Contenido { get; set; }
    }

}
