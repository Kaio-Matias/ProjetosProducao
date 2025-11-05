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
using System.Threading.Tasks; // Adicionado para Task
using System; // Adicionado para Exception
using System.Collections.Generic; // Adicionado para Dictionary

namespace Valedourado.Supervisor.ViewModels
{
    [QueryProperty(nameof(OrdemProducao), "OrdemProducao")]
    public partial class OpDetailViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IFileSaver _fileSaver;
        
        private CancellationTokenSource _cancellationTokenSource;

        [ObservableProperty]
        int ordemProducao;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOpAberta))]
        [NotifyCanExecuteChangedFor(nameof(FecharOpCommand))]
        [NotifyCanExecuteChangedFor(nameof(CancelarOpCommand))] // Notifica o novo comando
        RelatorioOpCompletoDto? relatorio;

        [ObservableProperty]
        string paletesTitle;
        
        [ObservableProperty]
        private int _totalProduzidoDetalhamento;

        [ObservableProperty]
        private int _totalPerdidoDetalhamento;
        
        [ObservableProperty]
        private int _totalPerdas;

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
            
            if (value?.Detalhamentos != null && value.Detalhamentos.Any())
            {
                TotalProduzidoDetalhamento = value.Detalhamentos.Sum(d => d.EmbProduzidas);
                TotalPerdidoDetalhamento = value.Detalhamentos.Sum(d => d.EmbPerdidas);
            }
            else
            {
                TotalProduzidoDetalhamento = 0;
                TotalPerdidoDetalhamento = 0;
            }
            
            if (value?.Perdas != null && value.Perdas.Any())
            {
                TotalPerdas = value.Perdas.Sum(p => p.Quantidade);
            }
            else
            {
                TotalPerdas = 0;
            }
        }
        
        [RelayCommand]
        private async Task LoadOpDetailsAsync()
        {
            _cancellationTokenSource?.Cancel(); 
            _cancellationTokenSource = new CancellationTokenSource();

            await ExecuteAsync(async () =>
            {
                Relatorio = await _apiService.GetRelatorioCompletoOpAsync(OrdemProducao, _cancellationTokenSource.Token);
            }, $"Não foi possível carregar os detalhes da OP.");
        }

        [RelayCommand(CanExecute = nameof(IsOpAberta))]
        private async Task FecharOpAsync()
        {
            bool userConfirmed = await Shell.Current.DisplayAlert("Confirmar Ação", $"Você tem certeza que deseja FECHAR a Ordem de Produção Nº {OrdemProducao}?", "Sim, Fechar", "Cancelar");
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
        
        [RelayCommand(CanExecute = nameof(IsOpAberta))] 
        private async Task CancelarOpAsync()
        {
            bool userConfirmed = await Shell.Current.DisplayAlert("Confirmar Cancelamento", $"Você tem certeza que deseja CANCELAR a Ordem de Produção Nº {OrdemProducao}? Esta ação não pode ser desfeita.", "Sim, Cancelar", "Não");
            if (!userConfirmed) return;

            await ExecuteAsync(async () =>
            {
                var sucesso = await _apiService.CancelarProducaoAsync(OrdemProducao);
                if (sucesso)
                {
                    await Shell.Current.DisplayAlert("Sucesso", "Ordem de Produção cancelada com sucesso!", "OK");
                    await Shell.Current.GoToAsync(".."); 
                }
                else
                {
                    await Shell.Current.DisplayAlert("Falha", "Não foi possível cancelar a OP. Ela pode já estar fechada ou não foi encontrada.", "OK");
                }
            }, "Ocorreu um erro ao cancelar a OP.");
        }
        
        [RelayCommand]
        private async Task SharePdfAsync()
        {
            // Criar um CancellationTokenSource local para esta operação
            var pdfCts = new CancellationTokenSource();

            await ExecuteAsync(async () =>
            {
                // ===== CORRIGIDO (para CS7036): Passando o CancellationToken =====
                var pdfBytes = await _apiService.GetRelatorioPdfAsync(OrdemProducao, pdfCts.Token);
                
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
        
        public void Cleanup()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}