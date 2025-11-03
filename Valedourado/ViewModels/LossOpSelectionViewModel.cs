using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    public partial class LossOpSelectionViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // As propriedades 'IsBusy' e 'IsNotBusy' foram removidas,
        // pois agora são herdadas da BaseViewModel.

        public ObservableCollection<ProducaoDto> OpenProducoes { get; } = new();

        public LossOpSelectionViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

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
            await Shell.Current.GoToAsync($"{nameof(RecordLossesPage)}?OrdemProducao={selectedOp.OrdemProducao}");
        }
    }
}