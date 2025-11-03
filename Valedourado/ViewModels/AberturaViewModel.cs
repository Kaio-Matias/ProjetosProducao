using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Services;
using Valedourado.Shared.Dtos;

namespace Valedourado.ViewModels
{
    public partial class AberturaViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenProductionCommand))]
        private CadastroDto _selectedCadastro;

        [ObservableProperty] private string _maquina;
        [ObservableProperty] private string _unidade;
        [ObservableProperty] private string _status = "Aberto";
        [ObservableProperty] private string _dataHoraAbertura;

        // ======================= INÍCIO DA CORREÇÃO =======================

        [ObservableProperty]
        private string comentario; // Propriedade para o Editor

        [ObservableProperty]
        private string quantidadeInicial; // Propriedade para o Entry

        // ======================== FIM DA CORREÇÃO =========================

        public ObservableCollection<CadastroDto> Cadastros { get; } = new();

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
                    // Se 'Comentario' e 'QuantidadeInicial' precisarem ser enviados
                    // para a API, você precisará adicioná-los ao CreateProducaoDto
                    // e passá-los aqui. Ex:
                    // Comentario = this.Comentario,
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