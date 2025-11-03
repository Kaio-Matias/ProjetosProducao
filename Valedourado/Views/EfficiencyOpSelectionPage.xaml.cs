using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class EfficiencyOpSelectionPage : ContentPage
{
    private readonly EfficiencyOpSelectionViewModel _viewModel;

    public EfficiencyOpSelectionPage(EfficiencyOpSelectionViewModel viewModel)
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