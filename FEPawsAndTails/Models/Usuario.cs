using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEPawsAndTails.Models
{
    public class Usuario
    {
        public long idUsuario { get; set; }
        public long idRol { get; set; }
        public string usrNombre { get; set; }
        public string usrCorreo { get; set; }
        //public string usrPasswd { get; set; }
        //public string fecha { get; set; }
        
        public static Usuario FromServicio(FEPawsAndTails.BackendAPI.USUARIO user)
        {
            if (user == null) return null;

            return new Usuario
            {
                idUsuario = user.ID_USUARIO,
                idRol = user.ID_ROL,
                usrNombre = user.USR_NOMBRE,
                usrCorreo = user.USR_NOMBRE                
            };
        }
    }
}