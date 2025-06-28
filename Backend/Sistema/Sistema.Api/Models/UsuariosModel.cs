using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class UsuariosModel
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public int IdPersonal { get; set; }
        [Required]
        public string Usuario { get; set; }
        public string Password { get; set; }
        public bool ActualizarPassword { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Estado { get; set; }
        public string Rol { get; set; }
        public string Personal { get; set; }
    }
}
