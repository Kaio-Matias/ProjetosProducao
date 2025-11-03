using CommunityToolkit.Mvvm.Input;
using Valedourado.Services;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        public HomeViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task GoTo(string route)
        {
            if (string.IsNullOrWhiteSpace(route)) return;
            await Shell.Current.GoToAsync(route);
        }

        [RelayCommand]
        private async Task Logout()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}