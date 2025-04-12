<<<<<<< Updated upstream
﻿using System.Security.Claims;
=======
using System.Security.Claims;
>>>>>>> Stashed changes
using JarredsOrderHub.DbaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HistorialPagosPedidos()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var pagos = await _context.Pagos
                .Include(p => p.Pedido)
                .Where(p => p.Pedido.UsuarioId == int.Parse(usuarioId))
                .ToListAsync();

            return View(pagos);
        }

        public async Task<IActionResult> HistorialPagosPedidosAdministracion()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Pedido)
                .ToListAsync();

            return View(pagos);
        }
    }
}