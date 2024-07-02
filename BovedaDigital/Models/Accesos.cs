namespace SmcApi.Models
{
    public class Accesos
    {
        public int? Id { get; set; }
        public int CodigoComuna { get; set; }
        public string Nube { get; set; }
        public string Bucket { get; set; }
        public string Credenciales { get; set; }
    }
}
