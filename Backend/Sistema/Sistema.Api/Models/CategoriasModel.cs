using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class CategoriasModel
    {
        public int IdCategoria { get; set; }
        [Required]
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
