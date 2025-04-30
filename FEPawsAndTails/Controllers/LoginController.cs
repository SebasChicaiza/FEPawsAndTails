using FEPawsAndTails.BackendAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEPawsAndTails.Controllers
{
    public class LoginController : Controller
    {
        private WebServiceGestionSoapClient client = new WebServiceGestionSoapClient();
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult ValidarCliente(string correo, string password)
        {
            try
            {
                var respuesta = client.autenticarUsuario(correo, password);

                if (respuesta == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado." }, JsonRequestBehavior.AllowGet);
                }

                // Creamos un objeto con la info necesaria
                var usuario = new
                {
                    rol = respuesta.ROL,
                    id = respuesta.ID_USUARIO,
                    nombre = respuesta.USR_NOMBRE,
                    correo = respuesta.USR_CORREO
                };

                return Json(new { success = true, usuario }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Opción segura: loguear el error y retornar respuesta limpia
                return Json(new { success = false, message = "Error al autenticar usuario." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
