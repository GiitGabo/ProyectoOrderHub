using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public string Email { get; set; }

        public int? IdRol { get; set; }
        public int? IdHorario { get; set; }
        [ForeignKey("IdRol")]
        public virtual Rol? Rol { get; set; }
        [ForeignKey("IdHorario")]
        public virtual Horario? Horario { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un número positivo.")]
        public int? Salario { get; set; }

        public string? Contrasenia { get; set; }

        public Boolean estado { get; set; }

        // NUEVAS PROPIEDADES para ubicación actual:
        [Column(TypeName = "float")]
        public double? LatitudActual { get; set; }
        [Column(TypeName = "float")]
        public double? LongitudActual { get; set; }
    }
}