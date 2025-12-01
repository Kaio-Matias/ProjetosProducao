using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using System.Collections.ObjectModel; // Certifique-se que este using existe
using System;
using System.Threading.Tasks;

namespace Valedourado.Supervisor.ViewModels
{
    // ===== CORREÇÃO AQUI (CS0260): Adicionado "partial" =====
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

        // ===== CORREÇÃO AQUI (CS0246): Trocado "ObservableObjectCollection" por "ObservableCollection" =====
        public ObservableCollection<CadastroDto> Cadastros { get; } = new();

        public AberturaViewModel(IApiService apiService)
        {
            _apiService = apiService;
            var timer = new System.Threading.Timer(e => DataHoraAbertura = DateTime.Now.ToString("g"), null, 0, 1000);
        }

        // ===== CORREÇÃO AQUI (CS0122): Trocado "private" por "public" =====
        [RelayCommand]
        public async Task LoadCadastros()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                // Limpa a lista na thread principal para evitar erros
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Cadastros.Clear();
                });

                var cadastrosList = await _apiService.GetCadastrosAsync();

                // Popula a lista na thread principal
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var cadastro in cadastrosList)
                    {
                        Cadastros.Add(cadastro);
                    }
                });
            }
            catch (Exception ex)
            {
                // Garante que o alerta de erro rode na thread principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Erro", $"Não foi possível carregar os produtos: {ex.Message}", "OK");
                });
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

                // Garante que o alerta e a navegação rodem na thread principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Sucesso", $"Ordem de Produção Nº {producaoCriada.OrdemProducao} aberta!", "OK");
                    await Shell.Current.GoToAsync("..");
                });
            }
            catch (Exception ex)
            {
                // Garante que o alerta de erro rode na thread principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Erro", $"Não foi possível abrir a produção: {ex.Message}", "OK");
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanOpenProduction() => SelectedCadastro != null && !IsBusy;
    }
}