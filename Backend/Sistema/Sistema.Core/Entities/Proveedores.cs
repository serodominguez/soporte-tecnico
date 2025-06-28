using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Proveedores
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Contacto { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public ICollection<Ingresos> ingresos { get; set; }
        public ICollection<Licencias> licencias { get; set; }
    }
}
