using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class SeccionContenido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Seccion { get; set; }

        public string Contenido { get; set; }

        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }

        public string ArchivoPdf { get; set; }

        public string PreguntasFrecuentes { get; set; }

        public bool Estado { get; set; }
    }

}
