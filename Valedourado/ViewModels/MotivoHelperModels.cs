using CommunityToolkit.Mvvm.ComponentModel;

namespace Valedourado.ViewModels
{
    public partial class SelectableMotivoItem : BaseViewModel
    {
        [ObservableProperty]
        private string _motivo;

        [ObservableProperty]
        private bool _isSelected;
    }

    public partial class MotivoPerda : BaseViewModel
    {
        [ObservableProperty]
        private string _motivo;

        [ObservableProperty]
        private int _quantidade;
    }

    public partial class MotivoTempo : BaseViewModel
    {
        [ObservableProperty]
        private string _motivo;

        [ObservableProperty]
        private TimeSpan _tempo;
    }
}