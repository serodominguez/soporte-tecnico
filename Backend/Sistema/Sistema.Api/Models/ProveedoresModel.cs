using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class ProveedoresModel
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Contacto { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
