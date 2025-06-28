using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class DetalleIngresos
    {
        public int IdDetalleIngresos { get; set; }
        public int IdIngreso { get; set; }
        public int IdEquipo { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string CodigoActivo { get; set; }
        public Ingresos ingreso { get; set; }
        public Equipos equipo { get; set; }
    }
}
