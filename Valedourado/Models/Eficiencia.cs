using System.Text.Json.Serialization;

namespace Valedourado.Models
{
    public class Eficiencia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ordemProducao")]
        public int OrdemProducao { get; set; }

        [JsonPropertyName("motivo")]
        public string Motivo { get; set; }

        [JsonPropertyName("tempo")]
        public TimeSpan Tempo { get; set; }


        [JsonPropertyName("operador")]
        public string Operador { get; set; }
    }
}
