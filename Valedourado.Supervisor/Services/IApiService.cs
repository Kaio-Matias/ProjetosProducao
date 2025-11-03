using Valedourado.Shared.Dtos;

namespace Valedourado.Supervisor.Services
{
    public interface IApiService
    {
        Task<UsuarioDto> LoginAsync(int matricula);
        Task<UsuarioDto> RegisterAsync(CreateUsuarioDto novoUsuario);
        Task<DashboardDto> GetDashboardDataAsync(CancellationToken token = default);
        Task<List<ProducaoDto>> GetOpenProducoesAsync(CancellationToken token = default);
        Task<List<ProducaoDto>> GetClosedProducoesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken token = default);
        Task<RelatorioOpCompletoDto> GetRelatorioCompletoOpAsync(int ordemProducao, CancellationToken token = default);
        Task<bool> FecharProducaoAsync(int ordemProducao);
        Task<byte[]> GetRelatorioPdfAsync(int ordemProducao, CancellationToken token = default);
        Task<bool> CancelarProducaoAsync(int ordemProducao);
    }
}