using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class AberturaPage : ContentPage
{
    private readonly AberturaViewModel _viewModel;

    public AberturaPage(AberturaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Carrega os dados sempre que a página for exibida
        _viewModel.LoadCadastrosCommand.Execute(null);
    }
}
