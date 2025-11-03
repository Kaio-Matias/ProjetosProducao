// Arquivo: Valedourado.Shared/Dtos/RelatorioOpCompletoDto.cs

// REMOVEMOS QUALQUER 'using' que aponte para a API_PRODUCAO.
// Este arquivo agora só conhece a si mesmo.

using Valedourado.Shared.Dtos;

namespace Valedourado.Shared.Dtos
{
    // DTOs para as sub-listas do relatório.
    // Eles já estavam aqui, mas agora serão usados corretamente.
    public class DetalhamentoOpDto
    {
        public int Id { get; set; }
        public string? Operador { get; set; }
        public string? Turno { get; set; }
        public int EmbProcessadas { get; set; }
        public int EmbProduzidas { get; set; }
        public int EmbPerdidas { get; set; }
    }

    public class PerdasOpDto
    {
        public int Id { get; set; }
        public string? Motivo { get; set; }
        public int Quantidade { get; set; }
        public string? Operador { get; set; }
    }

    public class EficienciaOpDto
    {
        public int Id { get; set; }
        public string? Motivo { get; set; }
        public TimeSpan Tempo { get; set; }
        public string? Operador { get; set; }
    }

    // O DTO principal do relatório
    public class RelatorioOpCompletoDto
    {
        // Propriedades gerais da OP
        public ProducaoDto InfoGeral { get; set; }

        // CORREÇÃO: As listas agora usam os DTOs definidos acima,
        // em vez dos modelos da API.
        public List<DetalhamentoOpDto> Detalhamentos { get; set; }
        public List<PerdasOpDto> Perdas { get; set; }
        public List<EficienciaOpDto> Paradas { get; set; }
        public List<PaleteDto> Paletes { get; set; } // PaleteDto já é um DTO conhecido

        public RelatorioOpCompletoDto()
        {
            Detalhamentos = new List<DetalhamentoOpDto>();
            Perdas = new List<PerdasOpDto>();
            Paradas = new List<EficienciaOpDto>();
            Paletes = new List<PaleteDto>();
        }
    }
}