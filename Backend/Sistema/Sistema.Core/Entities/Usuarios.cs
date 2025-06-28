using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Usuarios
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Estado { get; set; }
        public int IdRol { get; set; }
        public int IdPersonal { get; set; }
        public Roles roles { get; set; }
        public Personal personales { get; set; }
        public ICollection<Ingresos> ingresos { get; set; }
        public ICollection<Documentos> documentos { get; set; }

    }
}
