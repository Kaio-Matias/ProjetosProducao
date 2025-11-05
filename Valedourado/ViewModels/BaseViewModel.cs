using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Valedourado.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        [RelayCommand]
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        // ===== MÉTODO ADICIONADO PARA CORRIGIR ERROS DE OVERRIDE =====
        /// <summary>
        /// Método virtual que pode ser substituído por ViewModels filhas
        /// para executar ações quando a página associada aparece.
        /// </summary>
        public virtual void OnAppearing()
        {
            // A implementação base não faz nada,
            // mas permite que as filhas a substituam (override).
        }
    }
}