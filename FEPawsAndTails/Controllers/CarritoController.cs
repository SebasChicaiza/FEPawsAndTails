using FEPawsAndTails.BackendAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEPawsAndTails.Controllers
{
    public class CarritoController : Controller
    {
        private WebServiceGestionSoapClient client = new WebServiceGestionSoapClient();
        // GET: Carrito
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearFacturaConDetalle(List<PRODUCTO> producto, string idUsuario)
        {
            Array productos = producto.ToArray();
            Array productosConCant = new Array[producto.Count,2];
            for (int i = 0; i < producto.Count; i++)
            {
                productosConCant[i, 0] = productos[0];
                productosConCant[i, 0] = productos[1];
            }
            var respuesta = client.realizarCompra(productosConCant, idUsuario);
            // Convierte la lista de objetos ImagenProducto en un ArrayOfString (del SOAP)
            var arrayImagenes = new ArrayOfString();
            arrayImagenes.AddRange(producto.IMAGEN.Select(i => i.IMG_URL));

            return RedirectToAction("Producto");
        }
    }
}