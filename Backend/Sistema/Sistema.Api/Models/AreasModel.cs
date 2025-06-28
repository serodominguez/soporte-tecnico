using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class AreasModel
    {
        public int IdArea { get; set; }
        [Required]
        public string Area { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
