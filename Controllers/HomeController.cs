using System;
using System.Collections.Generic;
using JarredsOrderHub.DbaseContext;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JarredsOrderHub.ViewModels;

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

            // Obtén las categorías únicas de los platillos
            var categorias = platillos
                .Select(p => p.Categoria)
                .Distinct()
                .ToList();

            var viewModel = new MenuViewModel
            {
                Platillos = platillos,
                Categorias = categorias
            };

            return View(viewModel);
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