using Valedourado.Supervisor.ViewModels;

namespace Valedourado.Supervisor.Views;

public partial class AberturaPage : ContentPage
{
    private readonly AberturaViewModel _viewModel;

    public AberturaPage(AberturaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // =================================================================
        // ===== CORREÇÃO DEFINITIVA AQUI =====
        //
        // NÃO use Task.Run(). Chame o COMANDO. 
        // O [RelayCommand] no ViewModel já gerencia a Task e as threads 
        // de forma segura.
        //
        _viewModel.LoadCadastrosCommand.Execute(null);
        //
        // =================================================================
    }
}