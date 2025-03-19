using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class Preguntas
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Pregunta { get; set; }

        [Required]
        public string Respuesta { get; set; }
    }
}
