using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    public partial class OpSelectionViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        // A propriedade IsBusy foi removida pois já existe na BaseViewModel

        public ObservableCollection<ProducaoDto> OpenProducoes { get; } = new();

        public OpSelectionViewModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        public async Task LoadOpenOps()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var producoes = await _apiService.GetOpenProducoesAsync();
                OpenProducoes.Clear();
                foreach (var p in producoes)
                {
                    OpenProducoes.Add(p);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro de Conexão", ex.Message, "OK");
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
            await Shell.Current.GoToAsync($"{nameof(DetalhamentoPage)}?OrdemProducao={selectedOp.OrdemProducao}");
        }
    }
}