using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class DetalleDocumentos
    {
        public int IdDetalleDocumentos { get; set; }
        public int IdDocumento { get; set; }
        public int IdEquipo { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string CodigoActivo { get; set; }
        public int PrecioCompra { get; set; }
        public Documentos documento { get; set; }
        public Equipos equipo { get; set; }
    }
}
