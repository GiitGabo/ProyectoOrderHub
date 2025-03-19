using System.Threading.Tasks;
using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JarredsOrderHub.ViewModels;

namespace JarredsOrderHub.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AdministrarPlatos()
        {
            var platillos = await _context.Platillos
                .Include(p => p.Categoria)
                .ToListAsync();
            return View(platillos);
        }

        public async Task<IActionResult> AdministrarCategorias()
        {
            var categorias = await _context.Categorias
                .ToListAsync();
            return View(categorias);
        }

        public async Task<IActionResult> Menu()
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

    }
}