// Modelo para RecuperacionContrasenia
using System;
using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Models
{
    public class RecuperacionContrasenia
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime FechaExpiracion { get; set; }
        public bool Utilizado { get; set; }
    }

    // Modelo para datos del formulario de recuperación
    public class RecuperacionViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; }
    }

    // Modelo para datos del formulario de reseteo
    public class ResetContraseniaViewModel
    {
        [Required(ErrorMessage = "El token es requerido")]
        public string Token { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("Contrasenia", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasenia { get; set; }
    }
}