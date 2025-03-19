using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidoService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetallePedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedidos()
        {
            return await _context.DetallePedidos.ToListAsync();
        }

        [HttpGet("detallePedido/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesByPedido(int pedidoId)
        {
            var detalles = await _context.DetallePedidos
                                         .Where(dp => dp.PedidoId == pedidoId)
                                         .ToListAsync();

            if (!detalles.Any())
            {
                return NotFound();
            }

            return detalles;
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedido>> CreateDetallePedido(List<DetallePedido> detallePedido)
        {
            foreach (var detalle in detallePedido)
            {
                _context.DetallePedidos.Add(detalle);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePedido(int id)
        {
            var detallePedido = await _context.DetallePedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            _context.DetallePedidos.Remove(detallePedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
