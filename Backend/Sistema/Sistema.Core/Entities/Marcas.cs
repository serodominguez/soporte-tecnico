using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Marcas
    {
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public ICollection<Equipos> equipos { get; set; }
    }
}
