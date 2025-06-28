using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class SeccionesModel
    {
        public int IdSeccion { get; set; }
        [Required]
        public string Seccion { get; set; }
        public string Direccion { get; set; }
        public int? PkSeccion { get; set; }
        public int? PkTienda { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public int IdArea { get; set; }
        public string Area { get; set; }
    }
}
