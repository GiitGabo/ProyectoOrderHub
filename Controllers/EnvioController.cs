using JarredsOrderHub.DbaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JarredsOrderHub.Controllers
{
    public class EnvioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnvioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult CalificarPedido()
        {
            return View();
        }

        public ActionResult EstadoPedido()
        {

            return View();
        }

        public ActionResult GestionProblemas()
        {

            return View();
        }

        public ActionResult ReportarProblema()
        {

            return View();
        }

        public ActionResult RetrasoEnvio()
        {

            return View();
        }

        public async Task<ActionResult> ListadoPedidos()
        {
            var pedidos = await _context.Pedidos
                                .Include(p => p.Detalles)
                                .ThenInclude(d => d.Platillo)
                                .ToListAsync();
            return View(pedidos);

        public ActionResult VisualizarRetroalimentaciones()
        {

            return View();
        }
    }
}