using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Define o ViewModel como o contexto de dados da página
    }
}
