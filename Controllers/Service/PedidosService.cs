using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosService : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsuarioService> _logger;
        private readonly EmailService _emailService;

        public PedidosService(ApplicationDbContext context, ILogger<UsuarioService> logger, EmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedidos>>> GetPedidos()
        {
            return await _context.Pedidos.Include(p => p.Detalles).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedidos>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                                .Include(p => p.Detalles)
                                    .ThenInclude(d => d.Platillo)
                                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }


        [HttpGet("Detalles/{id}")]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetalles(int id)
        {
            var detalles = await _context.DetallePedidos
                                               .Where(p => p.PedidoId == id)
                                               .Include(p => p.Platillo)
                                               .ToListAsync();

            if (!detalles.Any())
            {
                return NotFound();
            }

            return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult<Pedidos>> CreatePedido(Pedidos pedido)
        {
            try
            {
                var cupon = pedido.Cupon;
                pedido.Cupon = null;

                decimal descuento = 0;

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                if (pedido.CuponId.HasValue)
                {
                    descuento = pedido.Total * (cupon.Descuento / 100m);

                    _context.CuponClientes.Add(new CuponCliente
                    {
                        ClienteId = pedido.UsuarioId,
                        CuponId = pedido.CuponId.Value,
                        FechaUso = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                }

                var pago = new Pago
                {
                    PedidoId = pedido.Id,
                    Monto = pedido.Total - descuento,
                    Estado = pedido.MetodoPago == "Efectivo" ? "Pendiente" : "Pagado",
                    FechaPago = pedido.MetodoPago == "Efectivo" ? null : DateTime.Now.Date
                };

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                CompletarPedido(pedido.Id);

                return Ok(new { success = true, data = pedido.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpPut("actualizarEstado/{id}/{nuevoEstado}")]
        public async Task<IActionResult> UpdatePedido(int id, string nuevoEstado)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Actualizar estado del pedido
                var pedido = await _context.Pedidos.FindAsync(id);
                if (pedido == null) return NotFound("Pedido no encontrado");

                pedido.EstadoPedido = nuevoEstado;
                _context.Entry(pedido).State = EntityState.Modified;

                // Actualizar estado del pago correspondiente
                var pago = await _context.Pagos.FindAsync(id);
                if (pago != null)
                {
                    pago.Estado = "Pagado";
                    pago.FechaPago = DateTime.Now;
                    _context.Entry(pago).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("AsignarRepartidor/{id}/{idRepartidor}")]
        public async Task<IActionResult> AsignarRepartidor(int id, int idRepartidor)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();
            pedido.IdRepartidor = idRepartidor;
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpGet("enviarRecibo/{pedidoId}")]
        public async Task<IActionResult> CompletarPedido(int pedidoId)
        {
            try
            {
                var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Platillo)
                .Include(p => p.Cupon)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

                var usuario = await _context.Clientes.FindAsync(pedido.UsuarioId);

                await _emailService.EnviarReciboPedido(pedido, usuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }




    }
}