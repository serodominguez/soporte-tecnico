using System;
using System.ComponentModel.DataAnnotations;


namespace Sistema.Api.Models
{
    public class LicenciasModel
    {
        public int IdLicencia { get; set; }
        [Required]
        public string Programa { get; set; }
        public string Licencia { get; set; }
        public string TipoLicencia { get; set; }
        public int CantidadEquipos { get; set; }
        public string PrecioCompra { get; set; }
        public string FechaCompra { get; set; }
        public string FechaCaducidad { get; set; }
        public string FechaActivacion { get; set; }
        public string Estado { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string Moneda { get; set; }
        public string Comentarios { get; set; }
    }
}
