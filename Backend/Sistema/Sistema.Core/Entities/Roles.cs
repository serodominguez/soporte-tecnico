using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Core.Entities
{
    public class Roles
    {
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public ICollection<Usuarios> usuarios { get; set; }
    }
}
