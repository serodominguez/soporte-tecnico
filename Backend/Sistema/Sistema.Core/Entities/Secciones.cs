using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Secciones
    {
        public int IdSeccion { get; set; }
        public string Seccion { get; set; }
        public string Direccion { get; set; }
        public int? PkSeccion { get; set; }
        public int? PkTienda { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public int IdArea { get; set; }
        public Areas areas { get; set; }
        public ICollection<Personal> personales { get; set; }
        public ICollection<Equipos> equipos { get; set; }
        public ICollection<Documentos> documentos { get; set; }
    }
}
