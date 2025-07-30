using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class PedidoController : Controller
    {
        public ActionResult Carrito()
        {
            // Se obtiene el ID del empleado desde la sesión
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // Redirige al login u otra vista de acciones de usuario si no hay empleado autenticado
                return RedirectToAction("AccionesUsuario", "Usuario");
            }

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.UsuarioId = usuarioId;

            return View();
        }
    }
}