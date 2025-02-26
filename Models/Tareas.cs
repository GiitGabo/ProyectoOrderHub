using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Tareas
    {
        [Key]
        public int IdTarea { get; set; }
        public required string NombreTarea { get; set; }
        public required string Descripcion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int? IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado? Empleado { get; set; }
    }

}
