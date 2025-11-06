using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;  
namespace Valedourado.Supervisor.ViewModels
{
    public class AberturaViewModel: BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenProductionCommand))]
        private CadastroDto _selectedCadastro;

        [ObservableProperty] private string _maquina;
        [ObservableProperty] private string _unidade;
        [ObservableProperty] private string _status = "Aberto";
        [ObservableProperty] private string _dataHoraAbertura;
        [ObservableProperty] private string comentario;
        [ObservableProperty] private string quantidadeInicial;

        public ObservableObjectCollection<CadastroDto> Cadastros { get; } = new();
        public AberturaViewModel(IApiService apiService)
        {
            _apiService = apiService;
            var timer = new System.Threading.Timer(e => DataHoraAbertura = DateTime.Now.ToString("g"), null, 0, 1000);
        }

        [RelayCommand]
        private async Task LoadCadastros()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Cadastros.Clear();
                var cadastrosList = await _apiService.GetCadastrosAsync();
                foreach (var cadastro in cadastrosList)
                {
                    Cadastros.Add(cadastro);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível carregar os produtos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnSelectedCadastroChanged(CadastroDto value)
        {
            Maquina = value?.Maquina;
            Unidade = value?.Unidade;
        }

        [RelayCommand(CanExecute = nameof(CanOpenProduction))]
        private async Task OpenProduction()
        {
            IsBusy = true;
            try
            {
                var novaProducao = new CreateProducaoDto
                {
                    Produto = SelectedCadastro.Produto,
                    Maquina = this.Maquina,
                    Unidade = this.Unidade
                    
                };
                var producaoCriada = await _apiService.CreateProducaoAsync(novaProducao);
                await Shell.Current.DisplayAlert("Sucesso", $"Ordem de Produção Nº {producaoCriada.OrdemProducao} aberta!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível abrir a produção: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanOpenProduction() => SelectedCadastro != null && !IsBusy;
    }
}
}
