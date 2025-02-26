using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public required string Email { get; set; }

        public int? IdRol { get; set; }
        public int? IdHorario { get; set; }
        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }
        [ForeignKey("IdHorario")]
        public virtual Horario? Horario { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un número positivo.")]
        public int? Salario { get; set; }

        public string? Contrasenia { get; set; }
    }
}
