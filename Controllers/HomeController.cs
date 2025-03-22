using System;
using System.Collections.Generic;
using JarredsOrderHub.DbaseContext;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace JarredsOrderHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
             var platillos = await _context.Platillos
                .Include(p => p.Categoria)
                .Where(p => p.Activo && p.Categoria.Activa)
                .ToListAsync();
            return View(platillos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}