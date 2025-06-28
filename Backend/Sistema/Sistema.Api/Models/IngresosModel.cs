using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Api.Models
{
    public class IngresosModel
    {
        public int IdIngreso { get; set; }
        public string IdUsuario { get; set; }
        public int IdProveedor { get; set; }
        public string FechaIngreso { get; set; }
        public int? NumeroIngreso { get; set; }
        public string TipoComprobante { get; set; }
        public string NumeroComprobante { get; set; }
        public string NumeroOrden { get; set; }
        public string Autorizado { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public string Proveedor { get; set; }
        public string Usuario { get; set; }
        public List<DetalleIngresosModel> detalles { get; set; }

    }
}
