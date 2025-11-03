using Valedourado.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;

namespace Paletizacao.Services
{
    public class ApiService : IApiService
    {
        private static readonly HttpClient _httpClient;
        private static readonly string BaseApiUrl = ConfigurationManager.AppSettings["BaseApiUrl"] ?? "http://localhost:5000";

        static ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseApiUrl.Trim()) };
        }

        public ApiService() { }

        private async Task HandleApiError(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorMessage = $"Erro na API: {response.StatusCode}.";
            if (!string.IsNullOrWhiteSpace(errorContent))
            {
                try
                {
                    var problemDetails = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (problemDetails.TryGetProperty("title", out var title)) { errorMessage += $" {title.GetString()}"; }
                    else if (problemDetails.TryGetProperty("message", out var message)) { errorMessage += $" {message.GetString()}"; }
                    else { errorMessage += $" Detalhes: {errorContent}"; }
                }
                catch { errorMessage += $" Detalhes: {errorContent}"; }
            }
            throw new HttpRequestException(errorMessage);
        }

        public async Task<UsuarioDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/login", loginRequest);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<UsuarioDto> RegisterAsync(CreateUsuarioDto novoUsuario)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Usuario/register", novoUsuario);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<List<ProducaoDto>> GetOpenProducoesAsync()
        {
            var response = await _httpClient.GetAsync("/api/Producoes/abertas");
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<List<ProducaoDto>>();
        }

        public async Task<CadastroDto> GetCadastroPorCodBarraAsync(string codBarra)
        {
            var response = await _httpClient.GetAsync($"/api/Cadastro/barcode/{codBarra}");
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<CadastroDto>();
        }

        public async Task<List<PaleteDto>> GetPaletesPorOPAsync(int ordemProducao)
        {
            var response = await _httpClient.GetAsync($"/api/Paletizacao/op/{ordemProducao}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<PaleteDto>>();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<PaleteDto>();
            }
            else
            {
                await HandleApiError(response);
                return null; // ou lançar exceção
            }
        }

        public async Task<PaleteDto> SalvarPaleteAsync(CreatePaleteDto palete)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Paletizacao", palete);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<PaleteDto>();
        }

        public async Task<PaleteDto> UpdateQtdePaleteAsync(int paleteId, UpdateQtdePaleteDto updateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Paletizacao/{paleteId}", updateDto);
            if (!response.IsSuccessStatusCode) await HandleApiError(response);
            return await response.Content.ReadFromJsonAsync<PaleteDto>();
        }
    }
}