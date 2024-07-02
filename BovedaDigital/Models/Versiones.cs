namespace SmcApi.Models
{
    public class Versiones
    {
        public int? Id { get; set; }
        public int? IdArchivo { get; set; }
        public int? IdAccesos { get; set; }
        public string URLArchivo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? NumeroVersion { get; set; }
    }
}
