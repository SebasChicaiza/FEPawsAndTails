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

        [HttpPost]
        public ActionResult EditarProducto(PRODUCTO producto)
        {
            client.actualizarProducto(producto);
            return RedirectToAction("Producto");
        }

        [HttpPost]
        public ActionResult EliminarProducto(int id)
        {
            client.eliminarProducto(id);
            return RedirectToAction("Producto");
        }
        [HttpPost]
        public ActionResult GuardarProducto(FormCollection form)
        {
            /*var client = new WebServiceGestionSoapClient();
            var arrayImagenes = new ArrayOfString();*/            

            var producto = new PRODUCTO
            {
                ID_PRODUCTO = string.IsNullOrEmpty(form["ID_PRODUCTO"]) ? 0 : int.Parse(form["ID_PRODUCTO"]),
                ID_CATEGORIA = string.IsNullOrEmpty(form["PROD_CAT"]) ? 0 : int.Parse(form["PROD_CAT"]),
                PROD_NOMBRE = form["PROD_NOMBRE"],
                PROD_DESC = form["PROD_DESC"],
                PROD_PRECIO = decimal.Parse(form["PROD_PRECIO"]),
                PROD_STOCK = int.Parse(form["PROD_STOCK"])
            };
            client.actualizarProducto(producto);
            /*
            if (producto.ID_PRODUCTO == 0)
            {
                client.crearProducto(producto.CATEGORIA.ToString(), producto.PROD_NOMBRE, producto.PROD_DESC, (decimal)producto.PROD_PRECIO, (int)producto.PROD_STOCK, arrayImagenes);
            }
            else
            {
                client.actualizarProducto(producto);
            }*/

            return RedirectToAction("Producto");
        }
        

    }
}