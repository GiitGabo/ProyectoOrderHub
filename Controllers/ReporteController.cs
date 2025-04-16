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
        private readonly IExportacionService _exportacionService;

        public ReporteController(ReporteService reporteService, IExportacionService exportacionService)
        {
            _reporteService = reporteService;
            _exportacionService = exportacionService;
        }

        // Acción para generar reporte en PDF o Excel
        public async Task<IActionResult> GenerarReporteVentas(int tipoReporte)
        {
            // Obtener los reportes de ventas desde el servicio
            var reportes = await _reporteService.ObtenerReportesAsync();
            byte[] archivo = null;

            // Verifica el tipo de reporte y genera el archivo correspondiente
            if (tipoReporte == 1) // PDF
            {
                archivo = await _exportacionService.GenerarReporteVentasPDF(reportes);
            }
            else if (tipoReporte == 2) // Excel
            {
                archivo = await _exportacionService.GenerarReporteVentasExcel(reportes);
            }

            // Retorna el archivo generado como una respuesta de tipo "file"
            return File(archivo, "application/octet-stream", "Reporte_Ventas." + (tipoReporte == 1 ? "pdf" : "xlsx"));
        }
    }
}
