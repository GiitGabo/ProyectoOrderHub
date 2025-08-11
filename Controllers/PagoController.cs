
using System.Security.Claims;
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
            // Se obtiene el ID del empleado desde la sesión
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {

                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Debes autenticarte para acceder.";

                // Redirige al login u otra vista de acciones de usuario si no hay empleado autenticado
                return RedirectToAction("AccionesUsuario", "Usuario");
            }

            var pagos = await _context.Pagos
                .Include(p => p.Pedido)
                .Where(p => p.Pedido.UsuarioId == int.Parse(userId) &&
                            p.Estado.ToLower() == "pagado")
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

/*    FILTROS ADMINISTRACION<!-- Filtros -->
<div class="filtros">
<label for="fechaInicio">Fecha de Inicio</label>
<input type="date" id="fechaInicio" onchange="aplicarFiltros()">

<label for="fechaFin">Fecha de Fin</label>
<input type="date" id="fechaFin" onchange="aplicarFiltros()">

<label for="metodoPago">Método de Pago</label>
<select id="metodoPago" onchange="aplicarFiltros()">
<option value="">Todos</option>
<option value="Efectivo">Efectivo</option>
<option value="Tarjeta de Crédito">Tarjeta de Crédito</option>
<option value="PayPal">PayPal</option>
</select>

<label for="estadoPago">Estado del Pago</label>
<select id="estadoPago" onchange="aplicarFiltros()">
<option value="">Todos</option>
<option value="Pagado">Pagado</option>
<option value="Pendiente">Pendiente</option>
</select>
</div>
@if (Model.Count == 0)
{
<div class="no-pagos text-center" style="margin:30px 0px;">
<p>Aún no existen pagos registrados.</p>
</div>
}
else
{
}
*/