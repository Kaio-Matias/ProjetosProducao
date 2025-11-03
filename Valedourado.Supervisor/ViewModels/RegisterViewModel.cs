using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;

namespace Valedourado.Supervisor.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        [ObservableProperty] private string _nome;
        [ObservableProperty] private string _matricula;
        [ObservableProperty] private string _cargo = "Supervisor"; // Cargo travado

        public RegisterViewModel(IApiService apiService) => _apiService = apiService;

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Matricula))
            {
                await Application.Current.MainPage.DisplayAlert("Campos Vazios", "Nome e Matrícula são obrigatórios.", "OK");
                return;
            }

            if (!int.TryParse(Matricula, out int matriculaInt))
            {
                await Application.Current.MainPage.DisplayAlert("Matrícula Inválida", "A matrícula deve ser um número.", "OK");
                return;
            }

            // --- MELHORIA APLICADA: Uso do helper ExecuteAsync ---
            await ExecuteAsync(async () =>
            {
                var newUser = new CreateUsuarioDto { Nome = Nome, Matricula = matriculaInt, Cargo = Cargo };
                await _apiService.RegisterAsync(newUser);

                await Application.Current.MainPage.DisplayAlert("Sucesso", "Supervisor registrado com sucesso! Agora você pode fazer o login.", "OK");

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }, "Ocorreu um erro no registro. Verifique se a matrícula já existe.");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}