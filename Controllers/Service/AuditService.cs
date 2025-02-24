using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers.Service
{

    public class AuditService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAuditoria(
            string tipoEntidad,
            int entidadId,
            string accion,
            string usuario,
            string detallesCambios,
            string descripcion)
        {
            var auditLog = new AuditLog
            {
                TipoEntidad = tipoEntidad,
                EntidadId = entidadId,
                Accion = accion,
                Usuario = usuario,
                FechaAccion = DateTime.UtcNow,
                DetallesCambios = detallesCambios,
                Descripcion = descripcion,
            };

            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
