using System.Collections.Generic;
using System.Threading.Tasks;
using JarredsOrderHub.Models;

namespace JarredsOrderHub.Controllers.Service
{
    public interface IExportacionService
    {
        Task<byte[]> GenerarReporteVentasPDF(List<Reporte> reportes);
        Task<byte[]> GenerarReporteVentasExcel(List<Reporte> reportes);
    }
}
