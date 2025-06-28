using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Personal
    {
        public int IdPersonal { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        public string Tipo { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public int Carnet { get; set; }
        public string Direccion { get; set; }
        public string PkEmpleado { get; set; }
        public string Cuenta { get; set; }
        public string Estado { get; set; }
        public int IdSeccion { get; set; }
        public Secciones secciones { get; set; }
        public ICollection<Usuarios> usuarios { get; set; }
        public ICollection<Documentos> documentos { get; set; }
    }
}
