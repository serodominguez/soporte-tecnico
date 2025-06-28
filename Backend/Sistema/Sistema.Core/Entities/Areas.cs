using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Areas
    {
        public int IdArea { get; set; }
        public string Area { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public ICollection<Secciones> secciones { get; set; }
    }
}
