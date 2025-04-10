using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CuponService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ValidarCupon")]
        public async Task<ActionResult<object>> ValidarCupon(int usuarioId, string codigo)
        {
            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Codigo == codigo);

            if (cupon == null)
                return BadRequest(new { success = false, message = "Cupón no válido." });

            if (cupon.FechaExpiracion <= DateTime.Now)
                return BadRequest(new { success = false, message = "Cupón expirado." });

            bool usuarioYaUsoCupon = await _context.CuponClientes.AnyAsync(cu => cu.ClienteId == usuarioId && cu.CuponId == cupon.Id);

            if (usuarioYaUsoCupon)
                return BadRequest(new { success = false, message = "Este cupón ya ha sido utilizado." });

            return Ok(new
            {
                success = true,
                cupon
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupon>>> GetCupones()
        {
            return await _context.Cupones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cupon>> GetCupon(int id)
        {
            var cupon = await _context.Cupones.FindAsync(id);
            if (cupon == null)
            {
                return NotFound();
            }
            return cupon;
        }

        [HttpPost]
        public async Task<ActionResult<Cupon>> CreateCupon(Cupon cupon)
        {
            cupon.Codigo = GenerarCodigoUnico();
            _context.Cupones.Add(cupon);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCupon", new { id = cupon.Id }, cupon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCupon(int id, Cupon cupon)
        {
            if (id != cupon.Id)
            {
                return BadRequest();
            }
            _context.Entry(cupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupon(int id)
        {
            var cupon = await _context.Cupones.FindAsync(id);
            if (cupon == null)
            {
                return NotFound();
            }
            _context.Cupones.Remove(cupon);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private string GenerarCodigoUnico()
        {
            string codigo;
            do
            {
                codigo = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            } while (_context.Cupones.Any(c => c.Codigo == codigo));
            return codigo;
        }
    }
}