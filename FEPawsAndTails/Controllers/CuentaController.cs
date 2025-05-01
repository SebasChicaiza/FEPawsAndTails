using FEPawsAndTails.BackendAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FEPawsAndTails.Models;

namespace FEPawsAndTails.Controllers
{
    public class CuentaController : Controller
    {
        private WebServiceGestionSoapClient client = new WebServiceGestionSoapClient();
        // GET: Cuenta
        public ActionResult Index()        {
            
            //Llamar al servicio
            var respuesta = client.obtenerClientes();

            return View("Micuenta");
        }
        public ActionResult Cuenta()
        {
            return View("Micuenta");
        }
        public ActionResult Registro()
        {
            return View("Registro");
        }
        [HttpPost]
        public JsonResult RegistrarUsuario(RegistroUsuarioDTO data)
        {
            try
            {
                var resultado = client.registrarUsuarioCliente(
                    data.nombreUsuario,
                    data.correo,
                    data.password,
                    data.nombreCliente,
                    data.apellidoCliente,
                    data.cedulaRuc,
                    data.telefono,
                    data.fechaNacimiento,
                    data.direccion
                );

                return Json(new { success = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}