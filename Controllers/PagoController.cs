﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class PagoController : Controller
    {
        public ActionResult HistorialPagosPedidos()
        {
            return View();
        }

        public ActionResult HistorialPagosPedidosAdministracion ()
        {

            return View();
        }


    }
}