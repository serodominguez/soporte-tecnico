using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Ingresos
    {
        public int IdIngreso { get; set; }
        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int NumeroIngreso { get; set; }
        public string TipoComprobante { get; set; }
        public int NumeroComprobante { get; set; }
        public string NumeroOrden { get; set; }
        public string Autorizado { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public ICollection<DetalleIngresos> detallesIngreso { get; set; }
        public Usuarios usuario { get; set; }
        public Proveedores proveedor { get; set; }
    }
}
