namespace SmcApi.Models
{
    public class Archivos
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Tema { get; set; }
        public string Glosa { get; set; }
        public string? RolesConsulta { get; set; }
        public string? RolesEscritura { get; set; }
        public int? comuna { get; set; }
        public List<Versiones>? Versiones { get; set; } = new List<Versiones>();

    }
}
