using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace FEPawsAndTails.Models
{
    public class ImagenProducto
    {
        public string ImgTipo { get; set; }
        public string ImgUrl { get; set; }
    }

    public class Producto
    {
        public long idProducto { get; set; }

        public long? idCat { get; set; }
        public string prodNombre { get; set; }
        public string prodDescripcion { get; set; }
        public decimal? prodPrecio { get; set; }
        public long? prodStock { get; set; }
        public string prodCat { get; set; }

        public List<ImagenProducto> Imagenes { get; set; } = new List<ImagenProducto>();

        // Método de mapeo estático
        public static Producto FromServicio(FEPawsAndTails.BackendAPI.PRODUCTO prodServicio)
        {
            if (prodServicio == null) return null;

            return new Producto
            {
                idProducto = prodServicio.ID_PRODUCTO,
                idCat = prodServicio.ID_CATEGORIA,
                prodCat = prodServicio.CATEGORIA.CAT_NOMBRE,
                prodNombre = prodServicio.PROD_NOMBRE,
                prodDescripcion = prodServicio.PROD_DESC,
                prodPrecio = prodServicio.PROD_PRECIO,
                prodStock = prodServicio.PROD_STOCK,
                Imagenes = new List<ImagenProducto>
                {
                    new ImagenProducto
                    {
                        ImgTipo = "PRI",
                        ImgUrl = prodServicio.IMAGEN.ToString(),
                    }
                }
            };
        }

     
    }

}