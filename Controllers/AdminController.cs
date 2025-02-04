using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace JarredsOrderHub.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AdministrarUsuarios()
        {
            return View();
        }

        public ActionResult AdministrarEmpleados()
        {
            return View();
        }

        public ActionResult AdministrarTareas()
        {

            return View();
        }

        public ActionResult AdministrarRoles()
        {

            return View();
        }
    }
}