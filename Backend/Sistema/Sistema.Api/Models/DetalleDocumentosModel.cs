using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Api.Models
{
    public class DetalleDocumentosModel
    {
        public int IdEquipo { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string CodigoActivo { get; set; }
        public int PrecioCompra { get; set; }
    }
}
