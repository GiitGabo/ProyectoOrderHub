// ReporteService.cs
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace JarredsOrderHub.Controllers.Service
{
    public class ReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context, ILogger<ReporteService> logger)
        {
            _context = context;
        }

        public async Task GuardarReporteAsync(Reporte reporte)
        {
            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reporte>> ObtenerReportesAsync()
        {
            return await _context.Reportes
                                 .Include(r => r.Cliente)
                                 .ToListAsync();
        }

        public async Task<Reporte> ObtenerReportePorIdAsync(int id)
        {
            return await _context.Reportes
                                 .Include(r => r.Cliente)
                                 .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task ActualizarReporteAsync(Reporte reporte)
        {
            _context.Reportes.Update(reporte);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reporte>> ObtenerReportesPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            return await _context.Reportes
                                 .Where(r => r.FechaReporte >= inicio && r.FechaReporte <= fin)
                                 .Include(r => r.Cliente)
                                 .ToListAsync();
        }
    }
}
