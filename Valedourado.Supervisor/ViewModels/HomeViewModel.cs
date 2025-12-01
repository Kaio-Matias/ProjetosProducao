using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.Views;

namespace Valedourado.Supervisor.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly IApiService _apiService;

        [ObservableProperty]
        private DashboardDto _dashboardData;

        [ObservableProperty]
        private string _welcomeMessage;

        // ESTA PROPRIEDADE NÃO É MAIS NECESSÁRIA AQUI, POIS A LISTA DE OPS ABERTAS
        // AGORA É RESPONSABILIDADE DA GestaoViewModel E HistoricoViewModel.
        // A HOMEPAGE É APENAS UM MENU.
        // public ObservableCollection<ProducaoDto> OpenOps { get; } = new();

        public HomeViewModel(IAuthService authService, IApiService apiService)
        {
            _authService = authService;
            _apiService = apiService;
            DashboardData = new DashboardDto();

            var currentUser = _authService.CurrentUser;
            if (currentUser?.Nome != null)
            {
                var firstName = currentUser.Nome.Split(' ').FirstOrDefault();
                WelcomeMessage = $"Bem-vindo, {firstName}";
            }
            else
            {
                WelcomeMessage = "Bem-vindo, Supervisor";
            }
        }

        // ESTE MÉTODO TAMBÉM NÃO É MAIS NECESSÁRIO NA HOMEVIEWMODEL,
        // POIS A HOMEPAGE AGORA É APENAS UM MENU E NÃO CARREGA DADOS DE LISTA.
        // CADA PÁGINA (GESTAO, HISTÓRICO) AGORA CARREGA SEUS PRÓPRIOS DADOS.
        /*
        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var dashboardTask = _apiService.GetDashboardDataAsync();
                var openOpsTask = _apiService.GetOpenProducoesAsync();
                await Task.WhenAll(dashboardTask, openOpsTask);
                DashboardData = dashboardTask.Result;
                OpenOps.Clear();
                foreach (var op in openOpsTask.Result) { OpenOps.Add(op); }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro de Dashboard", $"Não foi possível carregar os dados: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        */

        [RelayCommand]
        private async Task LogoutAsync()
        {
            _authService.Logout();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current.MainPage = IPlatformApplication.Current.Services.GetRequiredService<LoginPage>();
            });
        }

        // Comandos de navegação para os módulos
        [RelayCommand]
        private async Task GoToConsulta()
        {
            // CORRETO: Usa "//" porque HistoricoPage é uma ABA PRINCIPAL.
            await Shell.Current.GoToAsync($"//{nameof(HistoricoPage)}");
        }

        [RelayCommand]
        private async Task GoToAberturaOP()
        {
            // CORREÇÃO APLICADA:
            // Removemos o "//" para que a navegação seja relativa (empilhamento),
            // pois 'AberturaPage' está registrada como 'GestaoPage' (não é uma aba).
            await Shell.Current.GoToAsync(nameof(AberturaPage));
        }

        [RelayCommand]
        private async Task GoToGestao()
        {
            // CORRETO: Usa SEM "//" porque GestaoPage não é uma aba principal.
            // Estamos "empilhando" a página de gestão sobre a home.
            await Shell.Current.GoToAsync(nameof(GestaoPage));
        }
    }
}