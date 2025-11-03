using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    [QueryProperty(nameof(OrdemProducao), "OrdemProducao")]
    public partial class DetalhamentoViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;

        [ObservableProperty] private int _ordemProducao;
        [ObservableProperty] private string _operador;
        [ObservableProperty] private string _selectedTurno;

        [ObservableProperty][NotifyPropertyChangedFor(nameof(EmbPerdidas))] private int _embProcessadas;
        [ObservableProperty][NotifyPropertyChangedFor(nameof(EmbPerdidas))] private int _embProduzidas;

        public int EmbPerdidas => EmbProcessadas - EmbProduzidas;
        public List<string> Turnos { get; } = new List<string> { "1º Turno", "2º Turno", "3º Turno" };

        public DetalhamentoViewModel(IApiService apiService, IAuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
        }

        partial void OnOrdemProducaoChanged(int value) => Initialize();

        public void Initialize()
        {
            Operador = _authService.CurrentUser?.Nome;
            SelectedTurno = GetCurrentTurno();
        }

        private string GetCurrentTurno()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 6 && hour < 14) return "1º Turno";
            if (hour >= 14 && hour < 22) return "2º Turno";
            return "3º Turno";
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                var detalheDto = new CreateDetalhamentoOpDto
                {
                    OrdemProducao = this.OrdemProducao,
                    Operador = this.Operador,
                    Turno = this.SelectedTurno,
                    EmbProcessadas = this.EmbProcessadas,
                    EmbProduzidas = this.EmbProduzidas,
                    EmbPerdidas = this.EmbPerdidas
                };
                await _apiService.CreateDetalhamentoAsync(detalheDto);
                await Shell.Current.DisplayAlert("Sucesso", "Detalhamento salvo!", "OK");
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível salvar: {ex.Message}", "OK");
            }
        }
    }
}