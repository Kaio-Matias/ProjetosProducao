using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    public partial class PaletizacaoOpSelectionViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // Coleção observável para a lista na interface
        public ObservableCollection<ProducaoDto> Producoes { get; } = new();

        public PaletizacaoOpSelectionViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        // Este comando deve ser chamado no evento OnAppearing da Page ou via RefreshView
        [RelayCommand]
        public async Task LoadProducoes()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Limpa a lista atual para evitar duplicatas
                Producoes.Clear();

                // Busca as OPs abertas na API
                // Certifique-se de que este método na API Service está apontando para o endpoint correto "/api/Producoes/abertas"
                var lista = await _apiService.GetOpenProducoesAsync();

                if (lista != null && lista.Any())
                {
                    // Ordena por data de abertura (mais recentes primeiro)
                    // Verifica se DataHoraAbertura é válida, senão usa DataInicio se existir ou padrão
                    var listaOrdenada = lista.OrderByDescending(x => x.DataHoraAbertura).ToList();

                    foreach (var op in listaOrdenada)
                    {
                        Producoes.Add(op);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log ou feedback visual de erro
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível carregar as OPs: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectProducao(ProducaoDto producao)
        {
            if (producao == null) return;

            try
            {
                // Serializa o objeto para passar como parâmetro de navegação
                string jsonOp = JsonSerializer.Serialize(producao);

                // Navega para a tela de registro de paletes
                await Shell.Current.GoToAsync($"{nameof(PaletizacaoPage)}?OrdemProducaoJson={jsonOp}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Falha ao selecionar OP: {ex.Message}", "OK");
            }
        }
    }
}