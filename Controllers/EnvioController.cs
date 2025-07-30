using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using JarredsOrderHub.DbaseContext;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JarredsOrderHub.ViewModels;

namespace JarredsOrderHub.Controllers
{
    public class EnvioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnvioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult CalificarPedido()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EstadoPedido(int id)
        {
            // Obtenemos el ID del usuario autenticado
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("AccionesUsuario", "Usuario");
            }

            // Filtramos los pedidos que pertenecen al usuario autenticado, incluyendo detalles y plato
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Platillo)
                .Where(p => p.UsuarioId == int.Parse(userId))
                .ToListAsync();

            var repartidores = await _context.Empleados
                          .Where(e => e.IdRol == 3 && !e.estado)
                          .ToListAsync();
            ViewBag.Repartidores = repartidores;

            return View(pedidos);
        }

        [HttpGet]
        public async Task<IActionResult> EstadoPedidoRepartidor()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("AccionesUsuario", "Usuario");
            }

            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Platillo)
                .Include(p => p.Cliente)
                .Where(p => p.IdRepartidor == int.Parse(userId) &&
                            p.EstadoPedido.ToLower() != "entregado")
                .ToListAsync();

            var repartidores = await _context.Empleados
                .Where(e => e.IdRol == 3 && !e.estado)
                .ToListAsync();

            ViewBag.Repartidores = repartidores;

            return View(pedidos);
        }


        [HttpPost]
        public async Task<IActionResult> MarcarEntregado(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            var pago = await _context.Pagos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            pedido.EstadoPedido = "Entregado";
            pago.FechaPago = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }

        public class UbicacionDto
        {
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
        }



        public ActionResult GestionProblemas()
        {

            return View();
        }

        public ActionResult RetrasoEnvio()
        {

            return View();
        }

        public async Task<ActionResult> ListadoPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Platillo)
                .Include(p => p.Cliente)
                .ToListAsync();

            // Obtén repartidores (empleados con IdRol == 3 y activos)
            var repartidores = await _context.Empleados
                          .Where(e => e.IdRol == 3 && !e.estado)
                          .ToListAsync();

            ViewBag.Repartidores = repartidores;

            return View(pedidos);
        }


        public ActionResult VisualizarRetroalimentaciones()
        {

            return View();
        }
    }
}