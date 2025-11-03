using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Valedourado.Shared.Dtos;
using Valedourado.Supervisor.Services;
using System.IO;
using System.Diagnostics;
using CommunityToolkit.Maui.Storage;
using System.Threading;
using System.Linq;

namespace Valedourado.Supervisor.ViewModels
{
    [QueryProperty(nameof(OrdemProducao), "OrdemProducao")]
    public partial class OpDetailViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IFileSaver _fileSaver;

        // --- MELHORIA APLICADA: Controle de cancelamento da tarefa ---
        private CancellationTokenSource _cancellationTokenSource;

        [ObservableProperty]
        int ordemProducao;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOpAberta))]
        [NotifyCanExecuteChangedFor(nameof(FecharOpCommand))]
        RelatorioOpCompletoDto? relatorio;

        [ObservableProperty]
        string paletesTitle;

        public bool IsOpAberta => Relatorio?.InfoGeral?.Status == "Aberto";

        public OpDetailViewModel(IApiService apiService, IFileSaver fileSaver)
        {
            _apiService = apiService;
            _fileSaver = fileSaver;
        }

        partial void OnOrdemProducaoChanged(int value)
        {
            Relatorio = null;
            _ = LoadOpDetailsAsync();
        }

        partial void OnRelatorioChanged(RelatorioOpCompletoDto? value)
        {
            if (value?.Paletes != null && value.Paletes.Any())
            {
                var qtdePadrao = value.Paletes.First().QtdePorPalete;
                PaletesTitle = $"Paletes Gerados ({qtdePadrao} un / palete)";
            }
            else
            {
                PaletesTitle = "Paletes Gerados";
            }
        }

        // --- MELHORIA APLICADA: Comando refatorado com CancellationToken e ExecuteAsync ---
        [RelayCommand]
        private async Task LoadOpDetailsAsync()
        {
            _cancellationTokenSource?.Cancel(); // Cancela a requisição anterior
            _cancellationTokenSource = new CancellationTokenSource();

            await ExecuteAsync(async () =>
            {
                Relatorio = await _apiService.GetRelatorioCompletoOpAsync(OrdemProducao, _cancellationTokenSource.Token);
            }, $"Não foi possível carregar os detalhes da OP.");
        }

        [RelayCommand(CanExecute = nameof(IsOpAberta))]
        private async Task FecharOpAsync()
        {
            bool userConfirmed = await Shell.Current.DisplayAlert("Confirmar Ação", $"Você tem certeza que deseja fechar a Ordem de Produção Nº {OrdemProducao}?", "Sim, Fechar", "Cancelar");
            if (!userConfirmed) return;

            await ExecuteAsync(async () =>
            {
                var sucesso = await _apiService.FecharProducaoAsync(OrdemProducao);
                if (sucesso)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Ordem de Produção fechada com sucesso!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Falha", "Não foi possível fechar a OP. Ela pode já estar fechada ou não foi encontrada.", "OK");
                }
            }, "Ocorreu um erro ao fechar a OP.");
        }

        [RelayCommand]
        private async Task SharePdfAsync()
        {
            await ExecuteAsync(async () =>
            {
                var pdfBytes = await _apiService.GetRelatorioPdfAsync(OrdemProducao);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    await Shell.Current.DisplayAlert("Erro", "O relatório PDF está vazio ou não pôde ser gerado.", "OK");
                    return;
                }

#if WINDOWS
                using var stream = new MemoryStream(pdfBytes);
                var fileName = $"Relatorio_OP_{OrdemProducao}.pdf";
                var fileSaverResult = await _fileSaver.SaveAsync(fileName, stream, CancellationToken.None);

                if (fileSaverResult.IsSuccessful)
                {
                    await Shell.Current.DisplayAlert("Sucesso", $"Arquivo salvo com sucesso em: {fileSaverResult.FilePath}", "OK");
                }
                else
                {
                    var errorMessage = string.IsNullOrWhiteSpace(fileSaverResult.Exception?.Message)
                        ? "A operação foi cancelada ou falhou."
                        : fileSaverResult.Exception.Message;
                    await Shell.Current.DisplayAlert("Falha ao Salvar", $"Não foi possível salvar o arquivo. {errorMessage}", "OK");
                }
#else
                var tempFilePath = Path.Combine(FileSystem.CacheDirectory, $"Relatorio_OP_{OrdemProducao}.pdf");
                await File.WriteAllBytesAsync(tempFilePath, pdfBytes);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = $"Relatório da OP {OrdemProducao}",
                    File = new ShareFile(tempFilePath, "application/pdf")
                });
#endif
            }, "Ocorreu um erro inesperado ao tentar compartilhar o PDF.");
        }

        // --- MELHORIA APLICADA: Método para limpar recursos e cancelar tarefas ---
        public void Cleanup()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}