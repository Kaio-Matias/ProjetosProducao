using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.Views;

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

        public HistoricoViewModel(IApiService apiService) => _apiService = apiService;

        [RelayCommand]
        private async Task SearchByDateRangeAsync()
        {
            await ExecuteAsync(async () =>
            {
                var results = await _apiService.GetClosedProducoesByDateRangeAsync(StartDate, EndDate);
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
            await Shell.Current.GoToAsync($"{nameof(OpDetailPage)}?OrdemProducao={op.OrdemProducao}");
        }
    }
}