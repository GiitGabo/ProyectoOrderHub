using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult AdministrarUsuarios()
        {
            return View();
        }

        public async Task<IActionResult> AdministrarEmpleados()
        {
            var empleados = await _context.Empleados
                .Include(e => e.Rol)
                .Include(e => e.Horario)
                .ToListAsync();
            ViewBag.Roles = await _context.Roles.ToListAsync();
            ViewBag.Horario = await _context.Horarios.ToListAsync();
            return View(empleados);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarEmpleado([FromBody] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                var existeEmail = await _context.Empleados.AnyAsync(e => e.Email == empleado.Email);
                if (existeEmail)
                {
                    return Json(new { success = false, message = "El email ya está en uso." });
                }

                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Datos inválidos" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return Json(new { success = false, message = "Empleado no encontrado" });
            }
            return Json(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEmpleado([FromBody] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
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

                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Datos inválidos" });
        }


        [HttpPost]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return Json(new { success = false, message = "Empleado no encontrado" });
            }
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }


        public ActionResult AdministrarTareas()
        {

            return View();
        }

        public async Task<IActionResult> AdministrarRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }
    }
}