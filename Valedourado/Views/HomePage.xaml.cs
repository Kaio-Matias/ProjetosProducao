using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class HomePage : ContentPage
{
    // O ViewModel é injetado automaticamente pelo sistema de dependências.
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();

        // Esta é a linha crucial que liga a View ao seu ViewModel.
        BindingContext = viewModel;
    }
}
