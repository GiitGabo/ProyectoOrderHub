using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.Extensions.Logging;

namespace JarredsOrderHub.Controllers
{
    public class InformacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InformacionController> _logger;

        public InformacionController(ApplicationDbContext context, ILogger<InformacionController> logger)
        {
            _context = context;
            _logger = logger;  
        }

        // Acción para mostrar la vista de gestión de la información del footer
        [HttpGet]
        public IActionResult GestionarInformacion()
        {
            var contacto = _context.Contacto.FirstOrDefault() ?? new Contacto();
            var terminos = _context.TerminosCondiciones.FirstOrDefault() ?? new TerminosCondiciones();
            var preguntas = _context.Preguntas.ToList();
            var acercaDe = _context.AcercaDe.FirstOrDefault() ?? new AcercaDe();

            var model = new FooterInfoViewModel
            {
                Contacto = contacto,
                Terminos = terminos,
                Preguntas = preguntas,
                AcercaDe = acercaDe
            };

            return View(model);
        }

        // Acción para guardar la información del footer
        [HttpPost]
        public async Task<IActionResult> GuardarInformacionFooter(
            string footerAbout,
            string footerTelefono,
            string footerEmail,
            string footerDireccion,
            string footerFAQ,
            IFormFile footerTérminos)
        {
            try
            {
                // Validar los datos recibidos
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Inicio del proceso de guardado.");

                    // Actualizar o agregar la información de contacto
                    var contacto = _context.Contacto.FirstOrDefault() ?? new Contacto();
                    contacto.Telefono = footerTelefono;
                    contacto.Email = footerEmail;
                    contacto.Direccion = footerDireccion;

                    if (contacto.Id == 0)
                    {
                        _logger.LogInformation("Creando nuevo contacto.");
                        _context.Contacto.Add(contacto);
                    }
                    else
                    {
                        _logger.LogInformation("Actualizando contacto existente.");
                        _context.Contacto.Update(contacto);
                    }

                    // Actualizar o agregar la sección "Acerca de"
                    var acercaDe = _context.AcercaDe.FirstOrDefault() ?? new AcercaDe();
                    acercaDe.Contenido = footerAbout;

                    if (acercaDe.Id == 0)
                    {
                        _logger.LogInformation("Creando nueva entrada 'Acerca de'.");
                        _context.AcercaDe.Add(acercaDe);
                    }
                    else
                    {
                        _logger.LogInformation("Actualizando entrada 'Acerca de'.");
                        _context.AcercaDe.Update(acercaDe);
                    }

                    // Actualizar o agregar las preguntas frecuentes
                    if (!string.IsNullOrEmpty(footerFAQ))
                    {
                        _logger.LogInformation("Agregando nueva pregunta frecuente.");
                        var nuevaPregunta = new Preguntas
                        {
                            Pregunta = footerFAQ,
                            Respuesta = "Esta es una respuesta por defecto"
                        };
                        _context.Preguntas.Add(nuevaPregunta);
                    }

                    // Subir el archivo de Términos y Condiciones (si se ha enviado)
                    if (footerTérminos != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", footerTérminos.FileName);
                        _logger.LogInformation($"Archivo PDF recibido: {footerTérminos.FileName}, guardando en {filePath}");

                        // Guardar el archivo PDF en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await footerTérminos.CopyToAsync(stream);
                        }

                        var terminos = _context.TerminosCondiciones.FirstOrDefault() ?? new TerminosCondiciones();
                        terminos.Contenido = footerTérminos.FileName; // Guardar solo el nombre del archivo

                        if (terminos.Id == 0)
                        {
                            _logger.LogInformation("Creando nuevo término y condiciones.");
                            _context.TerminosCondiciones.Add(terminos);
                        }
                        else
                        {
                            _logger.LogInformation("Actualizando términos y condiciones existentes.");
                            _context.TerminosCondiciones.Update(terminos);
                        }
                    }

                    // Guardar todos los cambios
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Información del footer guardada correctamente.");

                    // Devolver una respuesta de éxito
                    return Json(new { success = true, message = "La información del footer ha sido actualizada correctamente." });
                }

                // Si los datos no son válidos, devolver mensaje de error
                _logger.LogWarning("La validación de los datos falló.");
                return Json(new { success = false, message = "Hubo un error al procesar la información. Por favor, revisa los campos." });
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _logger.LogError($"Ocurrió un error: {ex.Message}", ex);
                return Json(new { success = false, message = "Ocurrió un error: " + ex.Message });
            }
        }
    }
}
