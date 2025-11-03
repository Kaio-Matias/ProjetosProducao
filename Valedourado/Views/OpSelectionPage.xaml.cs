using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class OpSelectionPage : ContentPage
{
    private readonly OpSelectionViewModel _viewModel;

    public OpSelectionPage(OpSelectionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadOpenOpsCommand.Execute(null);
    }
}