using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class LossOpSelectionPage : ContentPage
{
    private readonly LossOpSelectionViewModel _viewModel;

    public LossOpSelectionPage(LossOpSelectionViewModel viewModel)
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