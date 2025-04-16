
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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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

                // Crear claims y principal para la autenticación
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, cliente.Nombre),
                    new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                    new Claim("UserType", "Cliente")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString("UserName", cliente.Nombre);
                HttpContext.Session.SetString("UserType", "Cliente");
                HttpContext.Session.SetInt32("ClienteId", cliente.IdCliente);

                return RedirectToAction("Menu", "Catalogo");
            }

            // Verificamos si el empleado existe
            var empleado = await _usuarioService.VerificarLoginEmpleado(email, contrasena);
            if (empleado != null)
            {

               var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, empleado.Nombre),
                    new Claim("UserType", "Empleado")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString("UserName", empleado.Nombre);
                HttpContext.Session.SetString("UserType", "Empleado");
                HttpContext.Session.SetInt32("EmpleadoId", empleado.IdEmpleado);
                return RedirectToAction("Menu", "Catalogo");
            }

            ViewBag.Error = "Correo o contraseña incorrectos";
            return View("AccionesUsuario");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("AccionesUsuario");
        }



        //-----------------RECUPERACION DE CONTRASEÑA------------------------//
        // Acción GET para mostrar formulario de recuperación (mantiene tu nombre existente)
        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        // Acción POST para enviar correo 
        [HttpPost]
        public async Task<IActionResult> RestablecerConfirmar(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "El correo electrónico es requerido");
                return View("Recuperar");
            }

            var (success, message) = await _usuarioService.IniciarRecuperacionContrasenia(email);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("AccionesUsuario");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View("Recuperar");
            }
        }

        // Acción GET para formulario de restablecimiento de contraseña
        [HttpGet]
        public async Task<IActionResult> RestablecerContrasenia(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("AccionesUsuario");
            }

            var tokenValido = await _usuarioService.ValidarTokenRecuperacion(token, email);
            if (!tokenValido)
            {
                TempData["ErrorMessage"] = "El enlace de recuperación es inválido o ha expirado";
                return RedirectToAction("AccionesUsuario");
            }

            var viewModel = new ResetContraseniaViewModel
            {
                Token = token,
                Email = email
            };

            return View(viewModel);
        }

        // Acción POST para procesar reset de contraseña
        [HttpPost]
        public async Task<IActionResult> RestablecerContrasenia(ResetContraseniaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _usuarioService.ResetearContrasenia(
                model.Token, model.Email, model.Contrasenia);

            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction("AccionesUsuario");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }



    }
}
