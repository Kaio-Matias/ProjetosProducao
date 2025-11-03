using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class RecordEfficiencyPage : ContentPage
{
    public RecordEfficiencyPage(RecordEfficiencyViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}