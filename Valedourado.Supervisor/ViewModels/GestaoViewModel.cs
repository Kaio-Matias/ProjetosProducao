using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.Views;
using System.Threading; // <-- ADICIONADO
using System.Collections.Generic; // <-- ADICIONADO (para List)
using System.Diagnostics; // <-- ADICIONADO (para Debug)

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

        // ===== ADICIONADO =====
        private CancellationTokenSource _cancellationTokenSource;

        public GestaoViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [RelayCommand]
        private async Task LoadOpenProductionsAsync()
        {
            // ===== ADICIONADO (Boa prática para recarregar) =====
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
            }
            catch (ObjectDisposedException) { }


            await ExecuteAsync(async () =>
            {
                // ===== CORRIGIDO (passando o token) =====
                var results = await _apiService.GetOpenProducoesAsync(_cancellationTokenSource.Token);
                // Agora o compilador reconhece a propriedade OpenProductions
                OpenProductions = new ObservableCollection<ProducaoDto>(results ?? new List<ProducaoDto>());
            }, "Não foi possível carregar as OPs abertas.");
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(ProducaoDto op)
        {
            if (op == null) return;
            // Corrigindo a passagem de parâmetro para usar o DTO ou ID
            // O seu OpDetailViewModel espera "OrdemProducao"
            await Shell.Current.GoToAsync($"{nameof(OpDetailPage)}", new Dictionary<string, object>
            {
                { "OrdemProducao", op.OrdemProducao }
            });
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

        // Boa prática: Adicionar um método para cancelar tasks ao sair da página
        public void Cleanup()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
            }
            catch (ObjectDisposedException) { }
        }
    }
}