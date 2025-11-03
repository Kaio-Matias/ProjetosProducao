using Valedourado.Supervisor.ViewModels;
namespace Valedourado.Supervisor.Views;
public partial class LoginPage : ContentPage
{ 
    public LoginPage(LoginViewModel viewModel) 
    { InitializeComponent(); BindingContext = viewModel; } 
}
