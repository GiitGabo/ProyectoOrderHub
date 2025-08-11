
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

                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Error";
                    TempData["ToastMessage"] = "Las contraseñas no coinciden";

                    return View("AccionesUsuario", cliente);
                }

                if (ModelState.IsValid)
                {
                    var (success, message) = await _usuarioService.RegistrarCliente(cliente);
                    if (success)
                    {

                        TempData["ToastType"] = "success";
                        TempData["ToastTitle"] = "Exito al registrar";
                        TempData["ToastMessage"] = "Cuenta creada con exito, Inicie sesion.";

                        return RedirectToAction("AccionesUsuario");
                    }
                    else
                    {
                        // Añadir error específico del servicio
                        ModelState.AddModelError("", message);

                        TempData["ToastType"] = "error";
                        TempData["ToastTitle"] = "Error";
                        TempData["ToastMessage"] = $"Registro fallido: {message}";

                        _logger.LogWarning($"Registro fallido: {message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Loggear error completo
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = $"Registro fallido: {ex}";

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
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Correo y contraseña obligatorios";

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
                    new Claim("UserId", cliente.IdCliente.ToString()),
                    new Claim("UserType", "Cliente")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(18)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


                HttpContext.Session.SetString("UserName", cliente.Nombre);
                HttpContext.Session.SetString("UserType", "Cliente");
                HttpContext.Session.SetInt32("ClienteId", cliente.IdCliente);

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Inicio de sesion exitoso.";

                return RedirectToAction("Index", "Home");
            }

            // Verificamos si el empleado existe
            var empleado = await _usuarioService.VerificarLoginEmpleado(email, contrasena);
            if (empleado != null)
            {

            string rolString = empleado.IdRol.HasValue
                ? (empleado.IdRol.Value == 1 ? "Admin"
            : empleado.IdRol.Value == 2 ? "Cocinero"
            : empleado.IdRol.Value == 3 ? "Repartidor"
            : "Otro")
            : "SinRol";


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, empleado.Nombre),
                    // Claim para el id del empleado
                    new Claim("UserId", empleado.IdEmpleado.ToString()),
                    // Claim para el rol: puedes usar ClaimTypes.Role o un claim personalizado
                    new Claim(ClaimTypes.Role, rolString),
                    // Si deseas identificar el tipo de usuario, también puedes incluirlo
                    new Claim("UserType", "Empleado")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(18)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                HttpContext.Session.SetString("UserName", empleado.Nombre);
                HttpContext.Session.SetString("UserType", "Empleado");
                HttpContext.Session.SetInt32("EmpleadoId", empleado.IdEmpleado);

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Inicio de sesion de empleado exitoso.";

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Correo o contraseña incorrectos";
            return View("AccionesUsuario");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            TempData["ToastType"] = "success";
            TempData["ToastTitle"] = "Exito";
            TempData["ToastMessage"] = "Logout exitoso";

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
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "El correo electronico es requerido.";

                ModelState.AddModelError("", "El correo electrónico es requerido");
                return View("Recuperar");
            }

            var (success, message) = await _usuarioService.IniciarRecuperacionContrasenia(email);

            if (success)
            {
                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Correo de recuperacion enviado.";

                TempData["SuccessMessage"] = message;
                return RedirectToAction("AccionesUsuario");
            }
            else
            {

                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error al recuperar contraseña";

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
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "El enlace de recuperación es inválido o ha expirado.";

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
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Exito";
                    TempData["ToastMessage"] = "Restablecimiento de contraseña exitoso.";
                    TempData["SuccessMessage"] = message;
                return RedirectToAction("AccionesUsuario");
            }
            else
            {
                ModelState.AddModelError("", message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarUbicacion([FromBody] UbicacionDto ubicacion)
        {
            int? empleadoId = HttpContext.Session.GetInt32("EmpleadoId");
            if (!empleadoId.HasValue)
            {
                    TempData["ToastType"] = "error";
                    TempData["ToastTitle"] = "Error";
                    TempData["ToastMessage"] = "Empleado no autenticado.";

                    return Unauthorized("Empleado no autenticado");
            }
            var resultado = await _usuarioService.ActualizarUbicacionAsync(empleadoId.Value, ubicacion.Latitud, ubicacion.Longitud);
            if (resultado)
            {
                    TempData["ToastType"] = "success";
                    TempData["ToastTitle"] = "Exito";
                    TempData["ToastMessage"] = "Ubicacion actualizada.";

                    return Ok(new { success = true, message = "Ubicación actualizada" });
            }
            TempData["ToastType"] = "error";
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Empleado no encontrado.";

            return NotFound("Empleado no encontrado");
        }

        [HttpGet]
        public async Task<ActionResult> GestionPerfil()
        {
            // Determinar si es cliente o empleado usando Claims
            if (User.Identity.IsAuthenticated)
            {
                var tipoUsuario = User.FindFirst("UserType")?.Value; // Cambiado de "TipoUsuario" a "UserType"

                if (tipoUsuario == "Cliente")
                {
                    // Obtenemos el ID del cliente desde claims
                    var idCliente = int.Parse(User.FindFirst("UserId").Value); // Cambiado de "IdUsuario" a "UserId"
                    var cliente = await _usuarioService.ObtenerPerfilCliente(idCliente);

                    if (cliente == null)
                        return RedirectToAction("AccionesUsuario", "Usuario");

                    return View("GestionPerfilC", cliente);
                }
                else if (tipoUsuario == "Empleado")
                {
                    // Obtenemos el ID del empleado desde claims
                    var idEmpleado = int.Parse(User.FindFirst("UserId").Value); // Cambiado de "IdUsuario" a "UserId"
                    var empleado = await _usuarioService.ObtenerPerfilEmpleado(idEmpleado);

                    if (empleado == null)
                        return RedirectToAction("AccionesUsuario", "Usuario");

                    return View("GestionPerfilE", empleado);
                }
            }

            return RedirectToAction("AccionesUsuario", "Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfilCliente(Cliente cliente)
        {

            var resultado = await _usuarioService.ActualizarPerfilCliente(cliente);

            if (resultado.success)
            {
                // Obtener el cliente actualizado
                var clienteActualizado = await _usuarioService.ObtenerPerfilCliente(cliente.IdCliente);

                // Actualizar sesión/claims
                HttpContext.Session.SetString("UserName", clienteActualizado.Nombre);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, clienteActualizado.Nombre),
                new Claim("UserId", clienteActualizado.IdCliente.ToString()),
                new Claim(ClaimTypes.Role, "Cliente"),
                new Claim("UserType", "Cliente")
            };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Exito al actualizar.";

                TempData["SuccessMessage"] = resultado.message;
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error al actualizar.";

                TempData["ErrorMessage"] = resultado.message;
            }

            return RedirectToAction("GestionPerfil");
        }


    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfilEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos del formulario inválidos";
                return RedirectToAction("GestionPerfil");
            }

            var resultado = await _usuarioService.ActualizarPerfilEmpleado(empleado);

            if (resultado.success)
            {
                // Obtener el empleado actualizado
                var empleadoActualizado = await _usuarioService.ObtenerPerfilEmpleado(empleado.IdEmpleado);

                // Actualizar la sesión
                HttpContext.Session.SetString("UserName", empleadoActualizado.Nombre);

                // Regenerar los claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, empleadoActualizado.Nombre),
            new Claim("UserId", empleadoActualizado.IdEmpleado.ToString()),
            new Claim(ClaimTypes.Role, empleadoActualizado.Rol?.Nombre ?? "Sin Rol"),
            new Claim("UserType", "Empleado")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Actualizar la cookie de autenticación
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = true });
                TempData["ToastType"] = "success";
                TempData["ToastTitle"] = "Exito";
                TempData["ToastMessage"] = "Exito al actualizar.";

                TempData["SuccessMessage"] = resultado.message;
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error al actualizar.";

                TempData["ErrorMessage"] = resultado.message;
            }

            return RedirectToAction("GestionPerfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarEmailRestablecer()
        {
            // Obtén el id del cliente de los claims
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return RedirectToAction("AccionesUsuario", "Usuario");

            // Recupera el objeto Cliente para extraer su email
            var cliente = await _usuarioService.ObtenerPerfilCliente(int.Parse(userId));
            if (cliente == null)
            {
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "No se encontro el perfil.";

                TempData["ErrorMessage"] = "No se encontró tu perfil";
                return RedirectToAction("GestionPerfil");
            }

            // Dispara el envío de correo
            var (success, message) = await _usuarioService.IniciarRecuperacionContrasenia(cliente.Email);
            if (success)
                TempData["SuccessMessage"] = message;

            else
                TempData["ToastType"] = "error";
                TempData["ToastTitle"] = "Error";
                TempData["ToastMessage"] = "Error al enviar el correo.";
                TempData["ErrorMessage"] = message;

            return RedirectToAction("GestionPerfil");
        }

    }
}
