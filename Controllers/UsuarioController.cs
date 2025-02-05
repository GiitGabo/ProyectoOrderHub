
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using JarredsOrderHub.Controllers.Service;

namespace JarredsOrderHub.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(UsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult AccionesUsuario()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCliente(Cliente cliente)
        {
            try
            {
                // Validación de contraseñas
                var confirmarContrasenia = Request.Form["ConfirmarContrasenia"].ToString();
                if (cliente.Contrasenia != confirmarContrasenia)
                {
                    ModelState.AddModelError("ConfirmarContrasenia", "Las contraseñas no coinciden");
                    return View("AccionesUsuario", cliente);
                }

                if (ModelState.IsValid)
                {
                    var (success, message) = await _usuarioService.RegistrarCliente(cliente);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Registro exitoso. Inicie sesión.";
                        return RedirectToAction("AccionesUsuario");
                    }
                    else
                    {
                        // Añadir error específico del servicio
                        ModelState.AddModelError("", message);
                        _logger.LogWarning($"Registro fallido: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Loggear error completo
                _logger.LogError(ex, "Error en registro de cliente");
                ModelState.AddModelError("", "Ocurrió un error inesperado. Intente nuevamente.");
            }

            return View("AccionesUsuario", cliente);
        }


    [HttpPost]
        public async Task<IActionResult> Login(string email, string contrasena)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.Error = "Correo y contraseña son obligatorios";
                return View("AccionesUsuario");
            }

            // Verificamos si el cliente existe
            var cliente = await _usuarioService.VerificarLoginCliente(email, contrasena);
            if (cliente != null)
            {
                HttpContext.Session.SetString("UserName", cliente.Nombre);
                HttpContext.Session.SetString("UserType", "Cliente");
                return RedirectToAction("Menu", "Catalogo");
            }

            // Verificamos si el empleado existe
            var empleado = await _usuarioService.VerificarLoginEmpleado(email, contrasena);
            if (empleado != null)
            {
                HttpContext.Session.SetString("UserName", empleado.Nombre);
                HttpContext.Session.SetString("UserType", "Empleado");
                return RedirectToAction("Menu", "Catalogo");
            }

            ViewBag.Error = "Correo o contraseña incorrectos";
            return View("AccionesUsuario");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AccionesUsuario");
        }

    }
}
