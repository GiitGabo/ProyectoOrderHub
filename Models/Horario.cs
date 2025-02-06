using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class Horario
    {
        [Key]  
        public int IdHorario { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
