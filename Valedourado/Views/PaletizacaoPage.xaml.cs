using Valedourado.ViewModels;

namespace Valedourado.Views
{
    public partial class PaletizacaoPage : ContentPage
    {
        public PaletizacaoPage(PaletizacaoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}