using Valedourado.Supervisor.ViewModels;

namespace Valedourado.Supervisor.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Apenas definimos o BindingContext
    }

    // O método OnAppearing foi removido pois não é mais necessário.
}