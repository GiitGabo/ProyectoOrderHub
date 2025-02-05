using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult AccionesUsuario()
        {
            return View();
        }

        public ActionResult GestionPerfil()
        {

            return View();
        }

    }
}