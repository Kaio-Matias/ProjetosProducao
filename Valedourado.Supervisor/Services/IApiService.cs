using Valedourado.Shared.Dtos;
using System; // Adicionado para DateTime
using System.Collections.Generic; // Adicionado para List
using System.IO;
using System.Threading; // Adicionado para CancellationToken
using System.Threading.Tasks; // Adicionado para Task

namespace Valedourado.Supervisor.Services
{
    public interface IApiService
    {
        // Assinatura corrigida para aceitar 'int' (matrícula)
        Task<UsuarioDto> LoginAsync(int matricula); 
        
        Task<UsuarioDto> RegisterAsync(CreateUsuarioDto registerRequest);
        Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken);
        Task<List<ProducaoDto>> GetOpenProducoesAsync(CancellationToken cancellationToken);
        
        // Assinatura corrigida para o histórico (baseado no erro CS0535)
        Task<List<ProducaoDto>> GetClosedProducoesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        
        Task<RelatorioOpCompletoDto> GetRelatorioCompletoOpAsync(int ordemProducao, CancellationToken cancellationToken);
        
        // Assinatura corrigida para incluir CancellationToken (baseado no erro CS0535)
        Task<byte[]> GetRelatorioPdfAsync(int ordemProducao, CancellationToken cancellationToken); 
        
        Task<bool> FecharProducaoAsync(int ordemProducao);
        
        // Novo método para cancelar OP
        Task<bool> CancelarProducaoAsync(int ordemProducao);

        Task<List<CadastroDto>> GetCadastrosAsync();
        Task<ProducaoDto> CreateProducaoAsync(CreateProducaoDto producao);

    }
}