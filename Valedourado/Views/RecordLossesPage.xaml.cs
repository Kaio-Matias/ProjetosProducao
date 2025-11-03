using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class RecordLossesPage : ContentPage
{
    public RecordLossesPage(RecordLossesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}