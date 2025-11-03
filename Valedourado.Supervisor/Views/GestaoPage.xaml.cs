using Valedourado.Supervisor.ViewModels;

namespace Valedourado.Supervisor.Views;

public partial class GestaoPage : ContentPage
{
    private readonly GestaoViewModel _viewModel;

    public GestaoPage(GestaoViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // CORREÇÃO: Chamar o comando diretamente.
        // O padrão async/await dentro do comando já garante que a UI não será bloqueada.
        _viewModel.LoadOpenProductionsCommand.Execute(null);
    }
}