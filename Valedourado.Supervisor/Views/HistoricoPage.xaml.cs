// Valedourado.Supervisor/Views/HistoricoPage.xaml.cs
using Valedourado.Supervisor.ViewModels;

namespace Valedourado.Supervisor.Views
{
    public partial class HistoricoPage : ContentPage
    {
        private readonly HistoricoViewModel _viewModel;
        public HistoricoPage(HistoricoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // MELHORIA: A busca inicial só é executada se a lista estiver vazia.
            // Isso evita chamadas desnecessárias à API toda vez que o usuário
            // alterna entre as abas, melhorando a performance e a experiência.
            if (_viewModel.ClosedProductions.Count == 0)
            {
                // O comando pode ser executado diretamente, pois já é assíncrono.
                await _viewModel.SearchByDateRangeCommand.ExecuteAsync(null);
            }
        }
    }
}