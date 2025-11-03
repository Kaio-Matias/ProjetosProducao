using Valedourado.Supervisor.ViewModels;

namespace Valedourado.Supervisor.Views;

public partial class OpDetailPage : ContentPage
{
    // --- MELHORIA APLICADA: Armazena a ViewModel para uso posterior ---
    private readonly OpDetailViewModel _viewModel;

    public OpDetailPage(OpDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // --- MELHORIA APLICADA: Cancela tarefas pendentes ao sair da página ---
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Cleanup();
    }
}