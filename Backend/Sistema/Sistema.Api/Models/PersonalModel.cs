using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class PersonalModel
    {
        public int IdPersonal { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Cargo { get; set; }
        public string Tipo { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Carnet { get; set; }
        public string Direccion { get; set; }
        public string PkEmpleado { get; set; }
        public string Personal { get; set; }
        public string Cuenta { get; set; }
        public string Estado { get; set; }
        public int IdSeccion { get; set; }
        public string Seccion { get; set; }

    }
}
