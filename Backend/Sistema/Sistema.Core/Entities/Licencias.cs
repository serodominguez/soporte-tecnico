using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Licencias
    {
        public int IdLicencia { get; set; }
        public string Programa { get; set; }
        public string Licencia { get; set; }
        public string TipoLicencia { get; set; }
        public int CantidadEquipos { get; set; }
        public int? PrecioCompra { get; set; }
        public DateTime? FechaCompra { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public string Estado { get; set; }
        public int IdProveedor { get; set; }
        public string Moneda { get; set; }
        public string Comentarios { get; set; }
        public Proveedores proveedor { get; set; }
    }
}
