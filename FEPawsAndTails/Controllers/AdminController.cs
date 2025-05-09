using FEPawsAndTails.BackendAPI;
using FEPawsAndTails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEPawsAndTails.Controllers
{
    public class AdminController : Controller
    {
        private WebServiceGestionSoapClient client = new WebServiceGestionSoapClient();
        // GET: Admin
        public ActionResult Index()
        {
            return View("Gestion");
        }
        public ActionResult Admin()
        {
            return View("Gestion");
        }
        public ActionResult Gestion()
        {
            return View("Gestion");
        }
        public ActionResult Factura()
        {
            return View(); // Ya no carga facturas por defecto
        }

        [HttpPost]
        public ActionResult BuscarFacturasPorCedula(string cedula)
        {
            var facturas = client.obtenerFacturasPorCedulaCliente(cedula);

            foreach (var f in facturas)
            {
                f.DETALLE_FACTURA = client.obtenerDetallesFactura((int)f.ID_FACTURA);
            }

            return PartialView("_FacturasPorCliente", facturas);
        }
        [HttpGet]
        public ActionResult ObtenerDetalles(int idFactura)
        {
            var detalles = client.obtenerDetallesFactura(idFactura);
            return PartialView("_DetalleFactura", detalles);
        }

        


        public ActionResult Producto()
        {
            //var client = new WebServiceGestionSoapClient();
            var respuesta = client.obtenerProductos();

            //Mapear la respuesta del WS a tu modelo
            var productos = respuesta.Select(prodServicio => new Producto
            {
                idProducto = prodServicio.ID_PRODUCTO,
                idCat = prodServicio.ID_CATEGORIA,
                prodNombre = prodServicio.PROD_NOMBRE,
                prodDescripcion = prodServicio.PROD_DESC,
                prodPrecio = prodServicio.PROD_PRECIO,
                prodStock = prodServicio.PROD_STOCK,
            }).ToList();

            return View("Producto", productos);
        }
        public JsonResult ObtenerProductos()
        {

            var productos = client.obtenerProductos();
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CrearProducto()
        {
            return View("Producto");
        }

        [HttpPost]
        public ActionResult CrearProducto(PRODUCTO producto)
        {
            // Convierte la lista de objetos ImagenProducto en un ArrayOfString (del SOAP)
            var arrayImagenes = new ArrayOfString();
            arrayImagenes.AddRange(producto.IMAGEN.Select(i => i.IMG_URL));

            client.crearProducto(producto.CATEGORIA.ToString(), producto.PROD_NOMBRE, producto.PROD_DESC, (decimal)producto.PROD_PRECIO, (int)producto.PROD_STOCK, arrayImagenes);
            return RedirectToAction("Producto");
        }

        public ActionResult EditarProducto(int id)
        {
            var producto = client.obtenerProductos().FirstOrDefault(p => p.ID_PRODUCTO == id);
            return View("Producto", producto);
        }

        /*[HttpPost]
        public ActionResult EditarProducto(PRODUCTO producto)
        {
            client.actualizarProducto(producto);
            return RedirectToAction("Producto");
        }*/

        [HttpPost]
        public ActionResult EliminarProducto(int id)
        {
            client.eliminarProducto(id);
            
            return RedirectToAction("Producto");
        }
        [HttpPost]
        public ActionResult GuardarProducto(FormCollection form)
        {
            var producto = new PRODUCTO
            {
                ID_PRODUCTO = string.IsNullOrEmpty(form["ID_PRODUCTO"]) ? 0 : int.Parse(form["ID_PRODUCTO"]),
                CATEGORIA = new BackendAPI.CATEGORIA
                {
                    CAT_NOMBRE = form["PROD_CAT"]
                },
                PROD_NOMBRE = form["PROD_NOMBRE"],
                PROD_DESC = form["PROD_DESC"],
                PROD_PRECIO = decimal.Parse(form["PROD_PRECIO"]),
                PROD_STOCK = int.Parse(form["PROD_STOCK"])
            };

            // URLs de imágenes ingresadas
            var imagenesRaw = form["IMAGENES"];
            var arrayImagenes = new ArrayOfString();
            if (!string.IsNullOrEmpty(imagenesRaw))
            {
                var urls = imagenesRaw.Split(',')
                    .Select(url => url.Trim())
                    .Where(url => !string.IsNullOrWhiteSpace(url));
                arrayImagenes.AddRange(urls);

                // También mapea a producto.IMAGEN si se desea usar en otra lógica
                producto.IMAGEN = urls.Select(url => new IMAGEN
                {
                    IMG_URL = url,
                    IMG_TIPO = "principal"
                }).ToArray();
            }

            if (producto.ID_PRODUCTO == 0)
            {
                // Crear nuevo
                client.crearProducto(
                    producto.CATEGORIA.CAT_NOMBRE,
                    producto.PROD_NOMBRE,
                    producto.PROD_DESC,
                    producto.PROD_PRECIO ?? 0m,
                    (int)(producto.PROD_STOCK ?? 0),
                    arrayImagenes
                );
            }
            else
            {
                // Actualizar existente
                client.actualizarProducto(
                    (int)producto.ID_PRODUCTO,
                    producto.CATEGORIA.CAT_NOMBRE,
                    producto.PROD_NOMBRE,
                    producto.PROD_DESC,
                    producto.PROD_PRECIO ?? 0m,
                    (int)(producto.PROD_STOCK ?? 0),
                    arrayImagenes
                );
            }

            return RedirectToAction("Producto");
        }

        //Facturas        

        [HttpPost]
        public JsonResult CambiarEstado(int idFactura, string nuevoEstado)
        {
            var resultado = client.actualizarEstadoFactura(idFactura, nuevoEstado);
            return Json(new { success = resultado });
        }
        [HttpPost]
        public JsonResult ActualizarEstadoFactura(int idFactura, string nuevoEstado)
        {
            try
            {
                bool result = client.actualizarEstadoFactura(idFactura, nuevoEstado);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}