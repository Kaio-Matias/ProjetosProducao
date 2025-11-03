using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class DetalhamentoPage : ContentPage
{
    public DetalhamentoPage(DetalhamentoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}