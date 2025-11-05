using Valedourado.Shared.Dtos;
using System.Net.Http.Json;
using System.Net.Http;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System; // Adicionado para DateTime

namespace Valedourado.Supervisor.Services
{
    // A classe agora implementa a IApiService corrigida
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public ApiService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task HandleApiError(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
        }

        // ===== IMPLEMENTAÇÃO CORRIGIDA (baseado no erro) =====
        public async Task<UsuarioDto> LoginAsync(int matricula)
        {
            // O DTO agora é criado aqui dentro
            var loginRequest = new LoginRequestDto { Matricula = matricula }; 
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/login", loginRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new HttpRequestException("Matrícula não encontrada.");
                await HandleApiError(response);
            }
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<UsuarioDto> RegisterAsync(CreateUsuarioDto registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/register", registerRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                    throw new HttpRequestException("Já existe um usuário com esta matrícula.");
                await HandleApiError(response);
            }
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<DashboardDto>("/api/Relatorio/dashboard", cancellationToken);
        }

        public async Task<List<ProducaoDto>> GetOpenProducoesAsync(CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<List<ProducaoDto>>("/api/Producoes/abertas", cancellationToken);
        }

        // ===== IMPLEMENTAÇÃO CORRIGIDA (baseado no erro) =====
        // (Este método substitui o antigo GetHistoricoProducoesAsync)
        public async Task<List<ProducaoDto>> GetClosedProducoesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            // Usa o endpoint da API que você já tinha
            var url = $"/api/Producoes/closed/bydate?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            return await _httpClient.GetFromJsonAsync<List<ProducaoDto>>(url, cancellationToken);
        }

        public async Task<RelatorioOpCompletoDto> GetRelatorioCompletoOpAsync(int ordemProducao, CancellationToken cancellationToken)
        {
            return await _httpClient.GetFromJsonAsync<RelatorioOpCompletoDto>($"/api/Relatorio/completo/{ordemProducao}", cancellationToken);
        }

        // ===== IMPLEMENTAÇÃO CORRIGIDA (baseado no erro) =====
        public async Task<byte[]> GetRelatorioPdfAsync(int ordemProducao, CancellationToken cancellationToken)
        {
            // Passa o CancellationToken para a chamada GetAsync
            var response = await _httpClient.GetAsync($"/api/Relatorio/pdf/{ordemProducao}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                await HandleApiError(response);
            }
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<bool> FecharProducaoAsync(int ordemProducao)
        {
            var response = await _httpClient.PutAsync($"/api/Producoes/{ordemProducao}/fechar", null);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Falha ao fechar OP {ordemProducao}. Status: {response.StatusCode}. Erro: {errorContent}");
            }
            
            return response.IsSuccessStatusCode;
        }

        // ===== NOVO MÉTODO ADICIONADO =====
        public async Task<bool> CancelarProducaoAsync(int ordemProducao)
        {
            var response = await _httpClient.PutAsync($"/api/Producoes/{ordemProducao}/cancelar", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Falha ao CANCELAR OP {ordemProducao}. Status: {response.StatusCode}. Erro: {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}