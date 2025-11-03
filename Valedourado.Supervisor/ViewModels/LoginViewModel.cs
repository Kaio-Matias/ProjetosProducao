// Valedourado.Supervisor/ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.Views;
using System.Net.Http;
using System.Net;

namespace Valedourado.Supervisor.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;
        [ObservableProperty] private string _matricula;

        public LoginViewModel(IAuthService authService, IApiService apiService)
        {
            _apiService = apiService;
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;
            if (!int.TryParse(Matricula, out var matriculaInt))
            {
                await Application.Current.MainPage.DisplayAlert("Entrada Inválida", "Por favor, informe uma matrícula numérica válida.", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                var user = await _apiService.LoginAsync(matriculaInt);
                if (!"Supervisor".Equals(user?.Cargo, StringComparison.OrdinalIgnoreCase))
                {
                    await Application.Current.MainPage.DisplayAlert("Acesso Negado", "Este usuário não tem permissão de supervisor.", "OK");
                    return;
                }

                _authService.Login(user);

                // CORREÇÃO: A navegação correta após o login é substituir a página principal
                // pela AppShell, que contém a estrutura de abas da aplicação.
                // É boa prática obter a instância do contêiner de DI.
                Application.Current.MainPage = IPlatformApplication.Current.Services.GetRequiredService<AppShell>();
            }
            // CORREÇÃO: Tratamento de erro mais específico com base na melhoria do ApiService.
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == HttpStatusCode.NotFound)
            {
                await Application.Current.MainPage.DisplayAlert("Erro de Login", "Matrícula não encontrada.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro Inesperado", $"Ocorreu um erro: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            // MELHORIA: Em vez de criar instâncias manualmente, resolvemos a página
            // a partir do contêiner de injeção de dependência. Isso garante que
            // todas as dependências (incluindo a ViewModel) sejam injetadas corretamente.
            var registerPage = IPlatformApplication.Current.Services.GetRequiredService<RegisterPage>();
            await Application.Current.MainPage.Navigation.PushModalAsync(registerPage);
        }
    }
}