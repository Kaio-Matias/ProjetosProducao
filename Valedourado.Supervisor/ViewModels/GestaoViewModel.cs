using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.Views;

namespace Valedourado.Supervisor.ViewModels
{
    public partial class GestaoViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // --- CORREÇÃO APLICADA: Propriedade implementada manualmente ---
        // Em vez de usar [ObservableProperty], criamos a propriedade completa.
        // Isso garante que o compilador sempre a encontrará.
        private ObservableCollection<ProducaoDto> _openProductions = new();
        public ObservableCollection<ProducaoDto> OpenProductions
        {
            get => _openProductions;
            set => SetProperty(ref _openProductions, value);
        }

        public GestaoViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task LoadOpenProductionsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var results = await _apiService.GetOpenProducoesAsync();
                // Agora o compilador reconhece a propriedade OpenProductions
                OpenProductions = new ObservableCollection<ProducaoDto>(results ?? new List<ProducaoDto>());
            }, "Não foi possível carregar as OPs abertas.");
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(ProducaoDto op)
        {
            if (op == null) return;
            await Shell.Current.GoToAsync($"{nameof(OpDetailPage)}?OrdemProducao={op.OrdemProducao}");
        }

        [RelayCommand]
        private async Task CancelProductionAsync(ProducaoDto op)
        {
            if (op == null) return;

            bool confirmed = await Shell.Current.DisplayAlert(
                "Confirmar Cancelamento",
                $"Tem certeza que deseja cancelar a OP Nº {op.OrdemProducao}?",
                "Sim, Cancelar", "Não");

            if (!confirmed) return;

            await ExecuteAsync(async () =>
            {
                var success = await _apiService.CancelarProducaoAsync(op.OrdemProducao);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Ordem de Produção cancelada.", "OK");
                    // O acesso à propriedade aqui também é corrigido.
                    OpenProductions.Remove(op);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Falha", "Não foi possível cancelar a OP.", "OK");
                }
            });
        }
    }
}