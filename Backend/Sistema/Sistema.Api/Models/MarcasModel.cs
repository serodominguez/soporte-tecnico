using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class MarcasModel
    {
        public int IdMarca { get; set; }
        [Required]
        public string Marca { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
