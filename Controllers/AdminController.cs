using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JarredsOrderHub.DbaseContext;
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

            return View(empleados);
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