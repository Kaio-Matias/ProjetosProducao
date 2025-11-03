using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    // Herda de BaseViewModel para reutilizar a propriedade IsBusy
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private bool isLoginViewVisible = true;

        [ObservableProperty]
        private bool isRegisterViewVisible = false;

        // A propriedade 'IsBusy' foi removida pois já existe na BaseViewModel.

        [ObservableProperty]
        private string matriculaLogin;

        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string cargo;

        [ObservableProperty]
        private string matriculaRegistro;

        public LoginViewModel(IApiService apiService, IAuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
        }

        [RelayCommand]
        private void ToggleView()
        {
            IsLoginViewVisible = !IsLoginViewVisible;
            IsRegisterViewVisible = !IsRegisterViewVisible;
        }

        [RelayCommand]

        private async Task ExecuteLogin()
        {
            if (IsBusy || string.IsNullOrWhiteSpace(MatriculaLogin)) return;

            try
            {
                if (!int.TryParse(MatriculaLogin, out int matricula))
                {
                    await Shell.Current.DisplayAlert("Erro", "A matrícula deve ser um número.", "OK");
                    return;
                }

                IsBusy = true;
                var usuario = await _apiService.LoginAsync(matricula);

                // ===== INÍCIO DA LÓGICA DE VALIDAÇÃO ATUALIZADA =====
                if (!"Operador".Equals(usuario?.Cargo, StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.DisplayAlert("Acesso Negado", "Seu cargo não permite o acesso a este aplicativo.", "OK");
                    return; // Interrompe o processo de login
                }
                // ===== FIM DA LÓGICA DE VALIDAÇÃO =====

                _authService.Login(usuario);
                MatriculaLogin = string.Empty;
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro de Login", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ExecuteRegister()
        {
            if (IsBusy) return;

            try
            {
                if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Cargo) || !int.TryParse(MatriculaRegistro, out int matricula))
                {
                    await Shell.Current.DisplayAlert("Erro de Validação", "Todos os campos são obrigatórios.", "OK");
                    return;
                }

                IsBusy = true;
                var novoUsuario = new UsuarioDto
                {
                    Nome = this.Nome,
                    Cargo = this.Cargo,
                    Matricula = matricula
                };

                await _apiService.RegisterAsync(novoUsuario);
                await Shell.Current.DisplayAlert("Sucesso", "Usuário registrado! Por favor, faça o login.", "OK");

                Nome = string.Empty;
                Cargo = string.Empty;
                MatriculaRegistro = string.Empty;
                ToggleView();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro de Registro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}