using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using JarredsOrderHub.Models;
using JarredsOrderHub.DbaseContext;

namespace JarredsOrderHub.Controllers
{
    public class EdicionInformacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EdicionInformacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Editar una sección
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var seccion = await _context.SeccionesContenido.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }

            return View(seccion); // Mostrar el contenido de la sección
        }

        // Guardar los cambios de una sección
        [HttpPost]
        public async Task<IActionResult> Editar(int id, SeccionContenido seccion, IFormFile archivoPdf)
        {
            if (ModelState.IsValid)
            {
                var seccionExistente = await _context.SeccionesContenido.FindAsync(id);
                if (seccionExistente == null)
                {
                    return NotFound();
                }

                // Si se ha subido un archivo PDF
                if (archivoPdf != null && archivoPdf.Length > 0)
                {
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fileName = Path.GetFileName(archivoPdf.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivoPdf.CopyToAsync(stream);
                    }

                    // Guardar la ruta del archivo PDF en la base de datos
                    seccion.ArchivoPdf = Path.Combine("uploads", fileName);
                }

                // Actualizar los otros campos de la sección
                seccionExistente.Contenido = seccion.Contenido;
                seccionExistente.Telefono = seccion.Telefono;
                seccionExistente.Email = seccion.Email;
                seccionExistente.Direccion = seccion.Direccion;
                seccionExistente.PreguntasFrecuentes = seccion.PreguntasFrecuentes;
                seccionExistente.ArchivoPdf = seccion.ArchivoPdf;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Detalles), new { id = seccionExistente.Id });
            }

            return View(seccion); // Si hay errores, mostrar de nuevo el formulario
        }

        // Eliminar una sección
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var seccion = await _context.SeccionesContenido.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }

            // Si existe un archivo PDF, eliminarlo del servidor
            if (!string.IsNullOrEmpty(seccion.ArchivoPdf))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", seccion.ArchivoPdf);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Eliminar la sección de la base de datos
            _context.SeccionesContenido.Remove(seccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirigir a la lista de secciones o página principal
        }

        // Previsualizar el contenido antes de guardarlo
        [HttpPost]
        public IActionResult Previsualizar(SeccionContenido seccion)
        {
            // Mostrar la previsualización del contenido editado sin guardarlo
            return View("Previsualizacion", seccion); // Muestra cómo quedará el contenido
        }

        // Detalles de una sección
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var seccion = await _context.SeccionesContenido.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }

            return View(seccion); // Mostrar los detalles de la sección
        }

        // Listado de todas las secciones
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var secciones = await _context.SeccionesContenido.ToListAsync();
            return View(secciones); // Mostrar todas las secciones editables
        }
    }
}