using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class InicioModel
    {
        [Required]
        public string usuario { get; set; }
        [Required]
        public string password { get; set; }
    }
}
