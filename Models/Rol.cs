using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Permisos { get; set; }
    }
}
