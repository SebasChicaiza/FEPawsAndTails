using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEPawsAndTails.Models
{
    public class RegistroUsuarioDTO
    {
        public string nombreUsuario { get; set; }
        public string correo { get; set; }
        public string password { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string cedulaRuc { get; set; }
        public string telefono { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public string direccion { get; set; }
    }
}