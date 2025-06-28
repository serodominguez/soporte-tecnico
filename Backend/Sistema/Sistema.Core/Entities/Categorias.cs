using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Categorias
    {
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public ICollection<Equipos> equipos { get; set; }
    }
}
