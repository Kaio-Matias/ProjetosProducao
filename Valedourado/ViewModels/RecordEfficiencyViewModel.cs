using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Valedourado.Services;
using Valedourado.Shared.Dtos;
using Valedourado.Views;

namespace Valedourado.ViewModels
{
    [QueryProperty(nameof(OrdemProducao), "OrdemProducao")]
    public partial class RecordEfficiencyViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;

        [ObservableProperty] private int _ordemProducao;
        [ObservableProperty] private string _operador;
        [ObservableProperty] private TimeSpan _totalTempo;

        public ObservableCollection<SelectableMotivoItem> AllMotivos { get; } = new();
        public ObservableCollection<MotivoTempo> SelectedMotivos { get; } = new();

        // ===== NOVA PROPRIEDADE ADICIONADA (CORRIGE ERRO CS0103) =====
        [ObservableProperty]
        private ObservableCollection<EficienciaDto> _registrosExistentes = new();

        public RecordEfficiencyViewModel(IApiService apiService, IAuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
            // Garante que 'Operador' nunca seja nulo
            Operador = _authService.CurrentUser?.Nome ?? "Operador";

            // Populando a lista de motivos de exemplo
            var motivosList = new List<string>
            {"ABAS ABERTAS",
            "ABERTURA DE TUBO",
            "ACUMULADOR",
            "AGUARDANDO CAIXA",
            "AGUARDANDO EMBALAGEM",
            "AGUARDANDO TURMA",
            "ANÁLISE DE LABORATÓRIO",
            "ANÁLISE DESTRUTIVA",
            "AR ESTERIL",
            "BOBINA DE TESTE",
            "CAP",
            "CIP",
            "CONGESTIONAMENTO FFU",
            "CQ",
            "DATADOR",
            "EMBALAGEM DANIFICADA",
            "EMENDA DE FÁBRICA",
            "ERRO DE CORREÇÃO",
            "ESTERILIZAÇÃO",
            "FALTA / PICO ENERGIA ELETRICA",
            "FALTA DE PROGRAMAÇÃO INTERNA",
            "FALTA ESPAÇO",
            "FALTA DE PRODUTO",
            "FINAL DE PRODUÃO",
            "FOTO CELULA",
            "FW 32",
            "HOMOGENIZADOR",
            "HORA DE REFEIÇÃO",
            "LABORATÓRIO",
            "LIMPEZA INTERMEDIARIA",
            "LIMPEZA SEMANAL",
            "LUBRIFICAÇÃO",
            "MAGAZINE",
            "MANDÍBULA",
            "MANUTENÇÃO CORRETiva",
            "MANUTENÇÃO PREVENTIVA",
            "MÁ FORMAÇÃO",
            "MATERIAL DE EMBALAGEM",
            "MOTOR",
            "OBSTRUÇÃO CALHA",
            "OBSTRUÇÃO SAÍDA FF",
            "OPERAÇÃO INCORRETA",
            "PARTIDA DE MÁQUINA",
            "PERÓXIDO",
            "PRENSAS",
            "PREPARAÇÃO",
            "PREPARAÇÃO/AGUARDANDO ESTERILIZAÇÃO",
            "PROBLEMA NA SL",
            "PROBLEMA NO APLICADOR DE FITA",
            "PRODUÇÃO",
            "ROLETE TRAVADO",
            "SHIRIK",
            "SIST. DE MOVIMENTO (CAMES)",
            "SISTEMA HIDRAULICO",
            "SOLDA TRASVERSAL",
            "SOLDAS AS/LS",
            "TETRA CARDBOARD",
            "TRANSPORTADOR (ESTEIRA)",
            "TROCA DE FITA",
            "TROCA DE PRODUTO",
            "TROCA DE BOBINA",
            "TUBEX",
            "UTILIDADES"};
            foreach (var motivo in motivosList)
            {
                AllMotivos.Add(new SelectableMotivoItem { Motivo = motivo });
            }
        }

        // ===== MÉTODO OnAppearing (AGORA CORRETO DEVIDO À MUDANÇA NO BASEVIEWMODEL) =====
        // (CORRIGE ERROS CS0117 e CS0115)
        public override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadDataAsync();
        }

        // ===== NOVO MÉTODO: PARA CARREGAR OS DADOS DA API =====
        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsBusy) return;

            if (OrdemProducao == 0 || string.IsNullOrEmpty(Operador))
            {
                return;
            }

            IsBusy = true;
            try
            {
                // RegistrosExistentes (com 'R' maiúsculo) é a propriedade pública gerada
                // (CORRIGE ERRO CS0103)
                RegistrosExistentes.Clear();

                // Chama o novo método da ApiService (CORRIGE ERRO CS1061)
                var registros = await _apiService.GetEficienciaPorOpEOperadorAsync(OrdemProducao, Operador);

                if (registros != null)
                {
                    foreach (var registro in registros)
                    {
                        // (CORRIGE ERRO CS0103)
                        RegistrosExistentes.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                // A ApiService já trata o 404 e retorna lista vazia
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível carregar lançamentos anteriores: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void ToggleMotivoSelection(SelectableMotivoItem motivoItem)
        {
            if (motivoItem == null) return;
            motivoItem.IsSelected = !motivoItem.IsSelected;

            if (motivoItem.IsSelected)
            {
                var novoMotivo = new MotivoTempo { Motivo = motivoItem.Motivo };
                novoMotivo.PropertyChanged += (s, e) => { if (e.PropertyName == nameof(MotivoTempo.Tempo)) UpdateTotal(); };
                SelectedMotivos.Add(novoMotivo);
            }
            else
            {
                var toRemove = SelectedMotivos.FirstOrDefault(m => m.Motivo == motivoItem.Motivo);
                if (toRemove != null) SelectedMotivos.Remove(toRemove);
            }
            UpdateTotal();
        }

        private void UpdateTotal() => TotalTempo = new TimeSpan(SelectedMotivos.Sum(m => m.Tempo.Ticks));

        // ===== MÉTODO SAVE ATUALIZADO =====
        [RelayCommand]
        private async Task Save()
        {
            try
            {
                var eficienciaParaEnviar = SelectedMotivos
                    .Where(m => m.Tempo.TotalSeconds > 0)
                    .Select(m => new EficienciaDto
                    {
                        OrdemProducao = this.OrdemProducao,
                        Motivo = m.Motivo,
                        Tempo = m.Tempo,
                        Operador = this.Operador
                    }).ToList();

                if (!eficienciaParaEnviar.Any())
                {
                    await Shell.Current.DisplayAlert("Atenção", "Nenhum registro de tempo foi feito.", "OK");
                    return;
                }

                await _apiService.CreateEficienciaAsync(eficienciaParaEnviar);
                await Shell.Current.DisplayAlert("Sucesso", "Registros de eficiência salvos!", "OK");

                // Limpa entradas e recarrega a lista
                ClearInputs();
                await LoadDataAsync();

                // Navegação removida
                // await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Não foi possível salvar: {ex.Message}", "OK");
            }
        }

        // ===== NOVO MÉTODO AUXILIAR PARA LIMPAR ENTRADAS =====
        private void ClearInputs()
        {
            SelectedMotivos.Clear();
            foreach (var motivo in AllMotivos.Where(m => m.IsSelected))
            {
                motivo.IsSelected = false;
            }
            UpdateTotal();
        }
    }
}