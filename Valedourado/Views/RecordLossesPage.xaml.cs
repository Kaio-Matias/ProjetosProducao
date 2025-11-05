using Valedourado.ViewModels;

namespace Valedourado.Views;

public partial class RecordLossesPage : ContentPage
{
    // Armazena uma referência à ViewModel para podermos chamá-la no OnAppearing
    private readonly RecordLossesViewModel _viewModel;

    public RecordLossesPage(RecordLossesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel; // Armazena a viewModel injetada
        BindingContext = _viewModel;
    }

    // ===== MÉTODO ADICIONADO =====
    /// <summary>
    /// Este é o método de ciclo de vida real da ContentPage.
    /// Ele é chamado automaticamente pelo .NET MAUI sempre que a página
    /// é exibida para o usuário.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Agora nós manualmente chamamos o método OnAppearing da nossa ViewModel,
        // que por sua vez executará o LoadDataAsync() para buscar os dados.
        _viewModel.OnAppearing();
    }
}