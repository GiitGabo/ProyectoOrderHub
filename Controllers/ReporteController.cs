using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult AdministracionReportes()
        {
            return View();
        }

        public ActionResult Reporte()
        {

            return View();
        }

    }
}