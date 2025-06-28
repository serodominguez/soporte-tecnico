using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Api.Models
{
    public class DocumentosModel
    {
        public int IdDocumento { get; set; }
        public string IdUsuario { get; set; }
        public int IdPersonal { get; set; }
        public int IdSeccion { get; set; }
        public string FechaEntrega { get; set; }
        public string FechaDevolucion { get; set; }
        public int? NumeroEntrega { get; set; }
        public int? NumeroDevolucion { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public string Personal { get; set; }
        public string Usuario { get; set; }
        public string Seccion { get; set; }
        public string Nombre { get; set; }
        public string PkEmpleado { get; set; }
        public string Cargo { get; set; }
        public string Celular { get; set; }
        public int Carnet { get; set; }
        public string Direccion { get; set; }
        public int Total { get; set; }
        public List<DetalleDocumentosModel> detalles { get; set; }
    }
}
