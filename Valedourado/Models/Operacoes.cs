using System.Text.Json.Serialization;

namespace Valedourado.Models
{
    public class Producoes
    {
        [JsonPropertyName("ordemProducao")]
        public int OrdemProducao { get; set; }

        [JsonPropertyName("produto")]
        public string? Produto { get; set; }

        [JsonPropertyName("maquina")]
        public string? Maquina { get; set; }

        [JsonPropertyName("unidade")]
        public string? Unidade { get; set; } // Propriedade adicionada

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("dataHoraAbertura")]
        public DateTime DataHoraAbertura { get; set; }

        [JsonPropertyName("dataHoraFechamento")]
        public DateTime? DataHoraFechamento { get; set; }
    }
}
