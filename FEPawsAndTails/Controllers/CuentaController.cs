using FEPawsAndTails.BackendAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEPawsAndTails.Controllers
{
    public class CuentaController : Controller
    {
        // GET: Cuenta
        public ActionResult Index()
        {
            var client = new WebServiceGestionSoapClient();

            //Llamar al servicio
            var respuesta = client.listarClientes();

            return View("Micuenta");
        }
        public ActionResult Cuenta()
        {
            return View("Micuenta");
        }
    }
}