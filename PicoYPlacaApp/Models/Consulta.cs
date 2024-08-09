namespace PicoYPlacaApp.Models
{
    public class Consulta
    {
        public string Placa { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Resultado { get; set; }
    }
}
