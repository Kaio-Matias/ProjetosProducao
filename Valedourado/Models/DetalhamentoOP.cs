using System.Text.Json.Serialization;

namespace Valedourado.Models
{
    public class DetalhamentoOP
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ordemProducao")]
        public int OrdemProducao { get; set; }

        [JsonPropertyName("operador")]
        public string Operador { get; set; }

        [JsonPropertyName("turno")]
        public string Turno { get; set; }

        [JsonPropertyName("embProcessadas")]
        public int EmbProcessadas { get; set; }

        [JsonPropertyName("embProduzidas")]
        public int EmbProduzidas { get; set; }

        [JsonPropertyName("embPerdidas")]
        public int EmbPerdidas { get; set; }
    }
}
