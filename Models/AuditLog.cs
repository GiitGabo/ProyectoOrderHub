using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoEntidad { get; set; } // "Platillo", "Categoria", "Empleado", etc.

        public int EntidadId { get; set; } // ID del objeto afectado

        [Required]
        [MaxLength(50)]
        public string Accion { get; set; } // "Creación", "Edición", "Eliminación"

        [Required]
        [MaxLength(100)]
        public string Usuario { get; set; } // Usuario que realizó la acción

        public DateTime FechaAccion { get; set; }

        [MaxLength(500)]
        public string DetallesCambios { get; set; } // JSON con los cambios realizados

        [MaxLength(255)]
        public string Descripcion { get; set; } // Descripción amigable de la acción

    
    }
}
