using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JarredsOrderHub.Controllers.Service;
using JarredsOrderHub.DbaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly AuditService _auditService;
        private readonly PedidosService _pedidosService;
        private readonly DetallePedidoService _detallePedidoService;

        public PagoController(ApplicationDbContext context, AuditService auditService, PedidosService pedidosService, DetallePedidoService detallePedidoService)
        {
            _context = context;
            _auditService = auditService;
            _pedidosService = pedidosService;
            _detallePedidoService = detallePedidoService;
        }

        [HttpGet]
        public async Task<ActionResult> HistorialPagosPedidos()
        {
            try
            {
                // Obtener el ID del cliente desde la sesión
                int? clienteId = HttpContext.Session.GetInt32("ClienteId");
                if (clienteId == null)
                {
                    // Si no hay un ID en sesión, devolver una vista con lista vacía o mensaje.
                    return View(new List<Pedidos>());
                }

                // Filtrar los pedidos del cliente con estado "entregado"
                var pedidos = await _context.Pedidos
                    .Include(p => p.Detalles)
                        .ThenInclude(d => d.Platillo)
                    .Include(p => p.Cliente)
                    .Where(p => p.UsuarioId == clienteId && p.EstadoPedido.ToLower() == "entregado")
                    .ToListAsync();

                return View(pedidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pedidos: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpGet]
        public async Task<ActionResult> HistorialPagosPedidosAdministracion()
        {
            try
            {
                // Se traen solo los pedidos entregados, se incluyen los detalles (con Platillo) y el usuario
                var pedidos = await _context.Pedidos
                    .Include(p => p.Detalles)
                        .ThenInclude(d => d.Platillo)
                    .Include(p => p.Cliente)
                    .Where(p => p.EstadoPedido.ToLower() == "entregado")
                    .ToListAsync();

                return View(pedidos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleados: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerDetallesPedido(int id)
        {
            try
            {
                var detalles = await _context.DetallePedidos
                    .Where(d => d.PedidoId == id)
                    .Include(d => d.Platillo)
                    .Select(d => new
                    {
                        id = d.Id,
                        pedidoId = d.PedidoId,
                        productoId = d.ProductoId,
                        nombrePlatillo = d.Platillo.Nombre,
                        cantidad = d.Cantidad,
                        precioUnitario = d.PrecioUnitario,
                        total = d.Total
                    })
                    .ToListAsync();

                return Json(detalles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener detalles del pedido: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    }
}