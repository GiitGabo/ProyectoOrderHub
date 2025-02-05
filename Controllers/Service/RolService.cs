using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ObtenerTodos()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet("ObtenerRol/{id}")]
        public async Task<Rol> ObtenerPorId(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> CrearRol([FromBody] Rol rol)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(rol);
                await _context.SaveChangesAsync();

                return Ok(rol);
            }
            return BadRequest("Error al crear el rol");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Rol rol)
        {
            var rolExistente = await _context.Roles.FindAsync(id);
            if (rolExistente == null)
            {
                return NotFound("Rol no encontrado");
            }

            rolExistente.Nombre = rol.Nombre;
            rolExistente.Descripcion = rol.Descripcion;
            rolExistente.Permisos = rol.Permisos;

            _context.Roles.Update(rolExistente);
            await _context.SaveChangesAsync();

            return Ok(rolExistente);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRol(int id)
        {
            var rol = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (rol == null)
            {
                return NotFound(new { mensaje = "Rol no encontrado" });
            }

            try
            {
                _context.Roles.Remove(rol);
                _context.SaveChanges();

                return Ok(new { mensaje = "Rol eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el rol", error = ex.Message });
            }
        }
    }
}
