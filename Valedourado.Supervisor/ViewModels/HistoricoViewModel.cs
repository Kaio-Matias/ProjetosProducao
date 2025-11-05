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
    public partial class HistoricoViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty] private DateTime _startDate = DateTime.Today.AddMonths(-1);
        [ObservableProperty] private DateTime _endDate = DateTime.Today;

        // --- CORREÇÃO APLICADA: Propriedade implementada manualmente ---
        private ObservableCollection<ProducaoDto> _closedProductions = new();
        public ObservableCollection<ProducaoDto> ClosedProductions
        {
            get => _closedProductions;
            set => SetProperty(ref _closedProductions, value);
        }

        // ===== ADICIONADO =====
        private CancellationTokenSource _cancellationTokenSource;

        public HistoricoViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [RelayCommand]
        private async Task SearchByDateRangeAsync()
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
                var results = await _apiService.GetClosedProducoesByDateRangeAsync(StartDate, EndDate, _cancellationTokenSource.Token);
                // O compilador agora reconhece a propriedade ClosedProductions
                ClosedProductions = new ObservableCollection<ProducaoDto>(results ?? new List<ProducaoDto>());

                if (ClosedProductions.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Sem Resultados", "Nenhuma OP fechada foi encontrada para o período.", "OK");
                }
            }, "Não foi possível realizar a busca.");
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