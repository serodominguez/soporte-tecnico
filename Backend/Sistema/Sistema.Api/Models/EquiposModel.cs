using System.ComponentModel.DataAnnotations;

namespace Sistema.Api.Models
{
    public class EquiposModel
    {
        public int IdEquipo { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string FechaCompra { get; set; }
        public int? PrecioCompra { get; set; }
        public int? MesesGarantia { get; set; }
        public string SistemaOperativo { get; set; }
        public string CodigoActivo { get; set; }
        public string NombreEquipo { get; set; }
        public string Procesador { get; set; }
        public string MemoriaRam { get; set; }
        public string Almacenamiento { get; set; }
        public string TarjetaVideo { get; set; }
        public string Condicion { get; set; }
        public string Asignado { get; set; }
        public string Estado { get; set; }
        public string Moneda { get; set; }
        public int Tipo { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int IdSeccion { get; set; }
        public string Seccion { get; set; }
    }
}
