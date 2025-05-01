using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEPawsAndTails.Models
{
    public class CompraRequestDTO
    {
        public List<DetalleCompraDTO> detalles { get; set; }
        public string direccion { get; set; }
        public string metodoPago { get; set; }
        public int idUsuario { get; set; }
    }
}