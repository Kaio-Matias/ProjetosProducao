using Valedourado.Shared.Dtos;

namespace Valedourado.Services
{
    public interface IApiService
    {
        Task<UsuarioDto> LoginAsync(int matricula);
        Task<UsuarioDto> RegisterAsync(UsuarioDto usuario);
        Task<List<CadastroDto>> GetCadastrosAsync();
        Task<ProducaoDto> CreateProducaoAsync(CreateProducaoDto producao);
        Task<List<ProducaoDto>> GetOpenProducoesAsync();
        Task CreateDetalhamentoAsync(CreateDetalhamentoOpDto detalhe);
        Task CreatePerdasAsync(List<PerdasDto> perdas);
        Task CreateEficienciaAsync(List<EficienciaDto> registos);

        // ===== MÉTODOS ADICIONADOS =====
        Task<List<PerdasDto>> GetPerdasPorOpEOperadorAsync(int ordemProducao, string operador);
        Task<List<EficienciaDto>> GetEficienciaPorOpEOperadorAsync(int ordemProducao, string operador);
    }
}