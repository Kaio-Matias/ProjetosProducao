using System.Net;
using System.Net.Http.Json;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Configuration; // --- MELHORIA APLICADA ---

namespace Valedourado.Supervisor.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient) => _httpClient = httpClient;

        // O método HandleApiError original é mantido sem alterações
        private async Task HandleApiError(HttpResponseMessage response, string defaultMessage = "Ocorreu um erro na API.")
        {
            if (response.IsSuccessStatusCode)
            {
                return; // Se a resposta foi bem-sucedida, não faz nada.
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            string errorMessage = string.IsNullOrWhiteSpace(errorContent) ? defaultMessage : errorContent;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Conflict: // 409
                    throw new HttpRequestException("Já existe um recurso com este identificador (Ex: Matrícula duplicada).", null, response.StatusCode);
                case HttpStatusCode.NotFound: // 404
                    throw new HttpRequestException("O recurso solicitado não foi encontrado.", null, response.StatusCode);
                case HttpStatusCode.Unauthorized: // 401
                case HttpStatusCode.Forbidden: // 403
                    throw new HttpRequestException("Acesso negado. Verifique suas permissões.", null, response.StatusCode);
                case HttpStatusCode.BadRequest: // 400
                    throw new HttpRequestException($"Dados inválidos: {errorMessage}", null, response.StatusCode);
                default:
                    throw new HttpRequestException(errorMessage, null, response.StatusCode);
            }
        }

        public async Task<UsuarioDto> LoginAsync(int matricula)
        {
            // --- MELHORIA APLICADA: Uso de constantes ---
            var response = await _httpClient.PostAsJsonAsync(AppConstants.ApiEndpoints.Login, new LoginRequestDto { Matricula = matricula });
            await HandleApiError(response, "Matrícula não encontrada ou erro no login.");
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<UsuarioDto> RegisterAsync(CreateUsuarioDto novoUsuario)
        {
            var response = await _httpClient.PostAsJsonAsync(AppConstants.ApiEndpoints.Register, novoUsuario);
            await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        // --- MELHORIA APLICADA: Uso de constantes e CancellationToken ---
        public async Task<DashboardDto> GetDashboardDataAsync(CancellationToken token = default) =>
            await _httpClient.GetFromJsonAsync<DashboardDto>(AppConstants.ApiEndpoints.Dashboard, token);

        public async Task<List<ProducaoDto>> GetOpenProducoesAsync(CancellationToken token = default) =>
            await _httpClient.GetFromJsonAsync<List<ProducaoDto>>(AppConstants.ApiEndpoints.ProducoesAbertas, token);

        public async Task<List<ProducaoDto>> GetClosedProducoesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken token = default)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");
            var url = string.Format(AppConstants.ApiEndpoints.ProducoesFechadasPorData, startDateString, endDateString);
            return await _httpClient.GetFromJsonAsync<List<ProducaoDto>>(url, token);
        }

        public async Task<RelatorioOpCompletoDto> GetRelatorioCompletoOpAsync(int ordemProducao, CancellationToken token = default) =>
            await _httpClient.GetFromJsonAsync<RelatorioOpCompletoDto>(string.Format(AppConstants.ApiEndpoints.RelatorioCompletoOp, ordemProducao), token);

        public async Task<bool> FecharProducaoAsync(int ordemProducao)
        {
            var response = await _httpClient.PutAsync(string.Format(AppConstants.ApiEndpoints.FecharProducao, ordemProducao), null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelarProducaoAsync(int ordemProducao)
        {
            var response = await _httpClient.PutAsync(string.Format(AppConstants.ApiEndpoints.CancelarProducao, ordemProducao), null);
            return response.IsSuccessStatusCode;
        }

        public async Task<byte[]> GetRelatorioPdfAsync(int ordemProducao, CancellationToken token = default) =>
            await _httpClient.GetByteArrayAsync(string.Format(AppConstants.ApiEndpoints.RelatorioPdf, ordemProducao), token);
    }
}