using JarredsOrderHub.Controllers.Service;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JarredsOrderHub.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ReporteService _reporteService;

        public ReporteController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public class EstadoUpdateModel
        {
            public int Id { get; set; }
            public string NuevoEstado { get; set; }
        }


        // Vista para realizar el reporte
        public ActionResult ReportarProblema()
        {
            return View();
        }

        //Guardar reporte

        [HttpPost]
        public async Task<IActionResult> GuardarReporte([FromBody] Reporte reporte)
        {
            if (reporte == null || string.IsNullOrEmpty(reporte.DescripcionReporte))
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                // Asigna la fecha
                reporte.FechaReporte = DateTime.Today;

                // Obtener el Id del cliente desde la sesión 
                var clienteId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(clienteId))
                {
                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Error";
                    TempData["ToastMessage"] = "No se encontro al cliente en la sesion";

                    return Json(new { success = false, message = "No se encontró el cliente en la sesión." });
                }
                else
                {
                    reporte.IdCliente = Convert.ToInt32(clienteId);
                }

                // Se fuerza el estado a "Pendiente"
                reporte.Estado = "Pendiente";

                await _reporteService.GuardarReporteAsync(reporte);


                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Reporte guardado correctamente.";

                return Json(new { success = true, message = "Reporte guardado correctamente." });
            }
            catch (Exception ex)
            {

                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = $"Error al obtener la guardar el reporte {ex.Message}.";

                return Json(new { success = false, message = "Error al guardar el reporte: " + ex.Message });
            }
        }

        // Acción para la administración de reportes
        public async Task<IActionResult> AdministracionReportes()
        {

            var reportes = await _reporteService.ObtenerReportesAsync();
            return View(reportes);
        }

        // Acción para actualizar el estado del reporte
        [HttpPost]
        public async Task<IActionResult> ActualizarEstado([FromBody] EstadoUpdateModel model)
        {
            if (model == null || model.Id <= 0 || string.IsNullOrEmpty(model.NuevoEstado))
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Datos invalidos";

                return Json(new { success = false, message = "Datos inválidos." });
            }

            try
            {
                var reporte = await _reporteService.ObtenerReportePorIdAsync(model.Id);
                if (reporte == null)
                {
                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Error";
                    TempData["ToastMessage"] = "Reporte no encontrado.";

                    return Json(new { success = false, message = "Reporte no encontrado." });
                }
                reporte.Estado = model.NuevoEstado;
                await _reporteService.ActualizarReporteAsync(reporte);

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Estado actualizado correctamente.";

                return Json(new { success = true, message = "Estado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = $"Error al obtener la guardar el reporte {ex.Message}.";

                return Json(new { success = false, message = "Error al actualizar el estado: " + ex.Message });
            }
        }
    }
}
