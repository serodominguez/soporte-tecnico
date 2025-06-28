using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class RolesModel
    {
        public int IdRol { get; set; }
        [Required]
        public string Rol { get; set; }
        public string Estado { get; set; }
    }
}
