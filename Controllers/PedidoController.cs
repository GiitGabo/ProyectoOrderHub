using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class PedidoController : Controller
    {
        public ActionResult Carrito()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.UsuarioId = usuarioId;

            return View();
        }
    }
}