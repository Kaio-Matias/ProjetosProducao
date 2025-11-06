using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Valedourado.Supervisor.Models
{
    /// <summary>
    /// Representa os dados de um produto cadastrado, vindo da API.
    /// </summary>
    public class Cadastro
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("codProduto")]
        public string CodProduto { get; set; }

        [JsonPropertyName("produto")]
        public string Produto { get; set; }

        [JsonPropertyName("codBarra")]
        public string CodBarra { get; set; }

        [JsonPropertyName("maquina")]
        public string Maquina { get; set; }

        [JsonPropertyName("unidade")]
        public string Unidade { get; set; }

        [JsonPropertyName("qtdeCaixa")]
        public int QtdeCaixa { get; set; }

        [JsonPropertyName("qtdePorPalete")]
        public int QtdePorPalete { get; set; }

        [JsonPropertyName("pesoBruto")]
        public double PesoBruto { get; set; }

        [JsonPropertyName("pesoLiquido")]
        public double PesoLiquido { get; set; }

        [JsonPropertyName("pesoTotalPalete")]
        public double PesoTotalPalete { get; set; }
    }
}
