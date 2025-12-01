using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;

namespace Valedourado.Supervisor.ViewModels
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
            // Este comando navega para a página anterior ("..")
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }
        // --- MELHORIA APLICADA: Método centralizado para executar comandos ---
        protected async Task ExecuteAsync(Func<Task> operation, string customErrorMessage = null)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await operation();
            }
            catch (HttpRequestException httpEx)
            {
                // Tratamento mais específico para erros de HTTP que já vêm do ApiService
                await Shell.Current.DisplayAlert("Erro de Comunicação", httpEx.Message, "OK");
            }
            catch (TaskCanceledException)
            {
                // Opcional: Tratar timeout de forma silenciosa ou com mensagem específica
                await Shell.Current.DisplayAlert("Erro de Conexão", "A requisição demorou muito para responder (timeout).", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro Inesperado", customErrorMessage ?? ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
            
        }
    }
}