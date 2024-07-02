namespace SmcApi.Models
{
    public class Blobs
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Glosa { get; set; }
        public string Extension { get; set; }
        public string Tema { get; set; }
        public string Id_comu { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Permitidos { get; set; }
        public string? Comuna { get; set; }
    }
}
