using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class Contacto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Direccion { get; set; }
    }

}
