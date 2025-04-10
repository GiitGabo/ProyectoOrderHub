using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PagosService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagos()
        {
            return await _context.Pagos.Include(p => p.Pedido).ToListAsync();
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagosUsuario(int usuarioId)
        {
            return await _context.Pagos
                .Include(p => p.Pedido)
                .Where(p => p.Pedido.UsuarioId == usuarioId && p.Estado == "Pagado")
                .ToListAsync();
        }

        [HttpPut("pagar/{id}")]
        public async Task<IActionResult> PagarPedido(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            pago.Estado = "Pagado";
            pago.FechaPago = DateTime.Now;

            _context.Entry(pago).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
