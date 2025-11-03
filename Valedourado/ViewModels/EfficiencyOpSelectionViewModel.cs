using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    public partial class EfficiencyOpSelectionViewModel(IApiService apiService) : BaseViewModel
    {
        private readonly IApiService _apiService = apiService;

        // As propriedades 'IsBusy' e 'IsNotBusy' foram removidas,
        // pois agora são herdadas da BaseViewModel.

        public ObservableCollection<ProducaoDto> OpenProducoes { get; } = new();

        [RelayCommand]
        private async Task LoadOpenOps()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var producoes = await _apiService.GetOpenProducoesAsync();
                OpenProducoes.Clear();
                foreach (var p in producoes) OpenProducoes.Add(p);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível carregar OPs: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SelectOp(ProducaoDto selectedOp)
        {
            if (selectedOp == null) return;
            await Shell.Current.GoToAsync($"{nameof(RecordEfficiencyPage)}?OrdemProducao={selectedOp.OrdemProducao}");
        }
    }
}