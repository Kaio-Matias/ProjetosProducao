using Valedourado.ViewModels;

namespace Valedourado.Views
{
    public partial class PaletizacaoOpSelectionPage : ContentPage
    {
        private readonly PaletizacaoOpSelectionViewModel _viewModel;

        // O parâmetro deve ser o ViewModel, NÃO a própria Page
        public PaletizacaoOpSelectionPage(PaletizacaoOpSelectionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Garante o carregamento dos dados ao entrar na tela
            _viewModel.LoadProducoesCommand.Execute(null);
        }
    }
}