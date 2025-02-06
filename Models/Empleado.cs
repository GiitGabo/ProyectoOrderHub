using System.ComponentModel.DataAnnotations;
    

namespace JarredsOrderHub.Models
{
    public class  Empleado
    {

        [Key]
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Email { get; set; }

        public int? IdRol { get; set; }
        public int? IdHorario { get; set; }

        public Rol Rol { get; set; } 
        public Horario Horario { get; set; } 
        public int? Salario { get; set; }

        public string Contrasenia { get; set; }

    }
}
