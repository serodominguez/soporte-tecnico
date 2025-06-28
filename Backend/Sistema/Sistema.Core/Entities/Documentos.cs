using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Documentos
    {
        public int IdDocumento { get; set; }
        public int IdPersonal { get; set; }
        public int IdUsuario { get; set; }
        public int IdSeccion { get; set; }
        public int NumeroEntrega { get; set; }
        public int? NumeroDevolucion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public int Total { get; set; }
        public ICollection<DetalleDocumentos> detallesDocumentos { get; set; }
        public Usuarios usuario { get; set; }
        public Personal personal { get; set; }
        public Secciones seccion { get; set; }
    }
}
