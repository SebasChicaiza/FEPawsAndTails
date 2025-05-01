using FEPawsAndTails.BackendAPI;
using FEPawsAndTails.Models;
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
        public JsonResult CrearFacturaConDetalle(CompraRequestDTO request)
        {
            try
            {
                // Crear y poblar el array especial del WS
                var productosArray = new BackendAPI.ArrayOfProductoCantidadDTO();

                foreach (var d in request.detalles)
                {
                    productosArray.Add(new BackendAPI.productoCantidadDTO
                    {
                        idProducto = d.idProducto,
                        cantidad = d.cantidad
                    });
                }

                // Crear el carrito con el tipo correcto
                var carrito = new BackendAPI.carritoDTO
                {
                    productos = productosArray
                };

                // Llamar al Web Service
                bool success = client.realizarCompra(carrito, request.direccion, request.metodoPago, request.idUsuario);

                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}