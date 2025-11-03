using System.Net;
using System.Net.Http.Json;
using Valedourado.Shared.Dtos;

namespace Valedourado.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient) { _httpClient = httpClient; }

        private async Task HandleApiError(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
        }
        //  Este endPont "/api/Usuario/login" é o endpoint que recebe a matrícula do usuário e retorna os dados do usuário se a matrícula for válida.
        public async Task<UsuarioDto> LoginAsync(int matricula)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/login", new LoginRequestDto { Matricula = matricula });
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<UsuarioDto> RegisterAsync(UsuarioDto usuario)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/register", usuario);
            if (response.StatusCode == HttpStatusCode.Conflict) throw new HttpRequestException("Já existe um usuário com esta matrícula.");
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<List<ProducaoDto>> GetOpenProducoesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProducaoDto>>("/api/Producoes/abertas");
        }

        public async Task<List<CadastroDto>> GetCadastrosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CadastroDto>>("/api/Cadastro");
        }

        public async Task<ProducaoDto> CreateProducaoAsync(CreateProducaoDto producao)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Producoes", producao);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<ProducaoDto>();
        }

        public async Task CreateDetalhamentoAsync(CreateDetalhamentoOpDto detalhe)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/DetalhamentoOP", detalhe);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
        }

        public async Task CreatePerdasAsync(List<PerdasDto> perdas)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Perdas", perdas);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
        }

        public async Task CreateEficienciaAsync(List<EficienciaDto> registos)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Eficiencia", registos);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
        }
    }
}