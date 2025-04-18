﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using JarredsOrderHub.Controllers.Service;
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JarredsOrderHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditService _auditService;
        private readonly UsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<UsuarioController> logger, AuditService auditService, UsuarioService usuarioService)
        {
            _context = context;
            _logger = logger;
            _auditService = auditService;
            _usuarioService = usuarioService;
        }
        // Acción para mostrar la lista de usuarios (clientes)
        [HttpGet]
        public async Task<IActionResult> AdministrarUsuarios()
        {
            try
            {
                // Obtener todos los clientes de la base de datos
                List<Cliente> usuarios = await _context.Clientes.ToListAsync();
                return View(usuarios);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdministrarEmpleados()
        {
            try
            {
                var empleados = await _context.Empleados
                    .Include(e => e.Rol)
                    .Include(e => e.Horario)
                    .ToListAsync();

                ViewBag.Roles = await _context.Roles.ToListAsync();
                ViewBag.Horario = await _context.Horarios.ToListAsync();

                return View(empleados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleados: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarEmpleado([FromBody] Empleado empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                // Verificar si ya existe un empleado con el mismo Nombre, Apellido o Email
                var empleadoDuplicado = await _context.Empleados.AnyAsync(e =>
                    e.Email == empleado.Email || (e.Nombre == empleado.Nombre && e.Apellido == empleado.Apellido));

                if (empleadoDuplicado)
                {

                    return Json(new { success = false, message = "El empleado ya está registrado." });
                }

                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();
                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _usuarioService.IniciarRecuperacionContrasenia(empleado.Email);

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Empleado",
                    entidadId: empleado.IdEmpleado,
                    accion: "Agregar Emp",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(empleado),
                    descripcion: $"Se agrego al empleado: {empleado.Nombre}"
                );


                return Json(new { success = true, message = "Empleado agregado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar empleado: {ex.Message}");
                return Json(new { success = false, message = "Error al agregar el empleado. Intente nuevamente." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleado(int id)
        {

            try
            {
                var empleado = await _context.Empleados
                    .Include(e => e.Rol)
                    .Include(e => e.Horario)
                    .FirstOrDefaultAsync(e => e.IdEmpleado == id);

                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado" });
                }

                return Json(new { success = true, data = empleado });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener empleado: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener el empleado." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEmpleado([FromBody] Empleado empleado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var empleadoExistente = await _context.Empleados.FindAsync(empleado.IdEmpleado);
                if (empleadoExistente == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado" });
                }

                empleadoExistente.Nombre = empleado.Nombre;
                empleadoExistente.Apellido = empleado.Apellido;
                empleadoExistente.Email = empleado.Email;
                empleadoExistente.IdRol = empleado.IdRol;
                empleadoExistente.IdHorario = empleado.IdHorario;
                empleadoExistente.Salario = empleado.Salario;

                await _context.SaveChangesAsync();

                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Empleado",
                    entidadId: empleado.IdEmpleado,
                    accion: "Editcion",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(empleado),
                    descripcion: $"Se edito al empleado con los siguientes datos: {empleado.Nombre}"
                );
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar empleado: {ex.Message}");
                return Json(new { success = false, message = "Error al actualizar el empleado." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);
                if (empleado == null)
                {
                    return Json(new { success = false, message = "Empleado no encontrado" });
                }

                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Empleado",
                    entidadId: empleado.IdEmpleado,
                    accion: "Eliminacion",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(empleado),
                    descripcion: $"Se elimino al empleado: {empleado.Nombre}"
                );
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
                return Json(new { success = false, message = "Error al eliminar el empleado." });
            }
        }

        [HttpGet]
        public IActionResult AdministrarTareas()
        {
            var tareas = _context.Tareas.Include(t => t.Empleado).ToList();
            ViewBag.Empleados = _context.Empleados.ToList();
            return View(tareas);
        }


        [HttpPost]
        public IActionResult AgregarTarea(Tareas nuevaTarea)
        {
            if (Request.Form["FechaEntrega"].ToString() == "")
            {
                nuevaTarea.FechaEntrega = null;
            }

            if (Request.Form["IdEmpleado"].ToString() == "")
            {
                nuevaTarea.IdEmpleado = null;
            }

            try
            {
                _context.Tareas.Add(nuevaTarea);
                var result = _context.SaveChanges();
                Console.WriteLine($"Registros guardados: {result}");
                return RedirectToAction("AdministrarTareas");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar: {ex.Message}");
                ModelState.AddModelError("", "Error al guardar la tarea: " + ex.Message);
            }

            ViewBag.Empleados = _context.Empleados.ToList();
            var tareas = _context.Tareas.Include(t => t.Empleado).ToList();
            return View("AdministrarTareas", tareas);
        }


        [HttpPost]
        public IActionResult EditarTarea(Tareas tareaEditada)
        {
            if (tareaEditada == null || tareaEditada.IdTarea == 0)
            {
                ModelState.AddModelError("", "Datos inválidos.");
            }

            var tareaExistente = _context.Tareas.Find(tareaEditada.IdTarea);
            if (tareaExistente == null)
            {
                ModelState.AddModelError("", "La tarea no existe.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = _context.Empleados.ToList();
                var tareas = _context.Tareas.Include(t => t.Empleado).ToList();
                return View("AdministrarTareas", tareas);
            }

            try
            {
                tareaExistente.NombreTarea = tareaEditada.NombreTarea;
                tareaExistente.Descripcion = tareaEditada.Descripcion;
                tareaExistente.FechaEntrega = tareaEditada.FechaEntrega;
                tareaExistente.IdEmpleado = tareaEditada.IdEmpleado == 0 ? null : tareaEditada.IdEmpleado;

                _context.SaveChanges();
                return RedirectToAction("AdministrarTareas");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la tarea: " + ex.Message);
                Console.WriteLine($"Error al actualizar tarea: {ex.Message}");
                ViewBag.Empleados = _context.Empleados.ToList();
                var tareas = _context.Tareas.Include(t => t.Empleado).ToList();
                return View("AdministrarTareas", tareas);
            }
        }

        [HttpPost]
        public IActionResult EliminarTarea(int id)
        {
            var tarea = _context.Tareas.Find(id);
            if (tarea == null)
            {
                TempData["Error"] = "La tarea no existe o ya fue eliminada.";
                return RedirectToAction("AdministrarTareas");
            }

            try
            {
                _context.Tareas.Remove(tarea);
                _context.SaveChanges();
                TempData["Success"] = "Tarea eliminada correctamente.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar tarea: {ex.Message}");
                TempData["Error"] = "No se pudo eliminar la tarea. Verifique si tiene dependencias.";
            }

            return RedirectToAction("AdministrarTareas");
        }


        public async Task<IActionResult> AdministrarRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }
    }
}