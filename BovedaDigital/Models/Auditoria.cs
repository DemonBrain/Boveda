namespace SmcApi.Models
{
    public class Auditoria
    {

        public int? Id { get; set; }
        public int? IdVersion{ get; set; }
        public int TipoAccion { get; set; }
        public string? CriterioBusqueda { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Identificador { get; set; }
        
    }
}
