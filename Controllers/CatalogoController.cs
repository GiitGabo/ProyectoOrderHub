using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class CatalogoController : Controller
    {
        public ActionResult AdministrarPlatos()
        {
            return View();
        }

        public ActionResult AdministrarCategorias()
        {

            return View();
        }

        public ActionResult Menu()
        {

            return View();
        }
    }
}