using FEPawsAndTails.BackendAPI;
using FEPawsAndTails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FEPawsAndTails.Controllers
{
    public class ProductosController : Controller
    {
        public ActionResult Index()
        {
            var client = new WebServiceGestionSoapClient();

            //Llamar al servicio
            var respuesta = client.obtenerProductos();

            //Mapear la respuesta del WS a tu modelo
            var productos = respuesta.Select(prodServicio => new Producto
            {
                idProducto = prodServicio.ID_PRODUCTO,
                prodNombre = prodServicio.PROD_NOMBRE,
                prodDescripcion = prodServicio.PROD_DESC,
                prodPrecio = prodServicio.PROD_PRECIO,
                prodStock = prodServicio.PROD_STOCK,

                //Mapeo de las imágenes, si hay
                Imagenes = prodServicio.IMAGEN != null
                    ? prodServicio.IMAGEN
                        .Where(img => img != null && !string.IsNullOrWhiteSpace(img.IMG_URL))
                        .Select(img => new ImagenProducto
                        {
                            ImgTipo = img.IMG_TIPO,
                            ImgUrl = img.IMG_URL
                        }).ToList()
                    : new List<ImagenProducto>()

            }).ToList();

            return View(productos);
        }
    }
}
