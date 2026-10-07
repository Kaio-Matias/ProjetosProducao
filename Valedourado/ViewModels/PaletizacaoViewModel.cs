using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;
using Valedourado.Services;
using Valedourado.Shared.Dtos;

namespace Valedourado.ViewModels
{
    [QueryProperty(nameof(OrdemProducaoJson), "OrdemProducaoJson")]
    public partial class PaletizacaoViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IAuthService _authService;
        private ProducaoDto _producaoAtual;

        // Propriedades da Tela
        [ObservableProperty] private string _tituloPagina = "Carregando...";
        [ObservableProperty] private int _totalProduzidoGeral;

        // Controles de Entrada
        [ObservableProperty] private bool _isUltimoPalete;

        // Definindo manualmente para garantir acesso e evitar erro CS0103
        private string _qtdeUltimoPaleteText;
        public string QtdeUltimoPaleteText
        {
            get => _qtdeUltimoPaleteText;
            set => SetProperty(ref _qtdeUltimoPaleteText, value);
        }

        [ObservableProperty] private bool _isBloqueado;
        [ObservableProperty] private string _motivoBloqueioSelecionado;

        public ObservableCollection<PaleteDto> Paletes { get; } = new();

        public List<string> MotivosBloqueio { get; } = new()
        {
            "FALTA CANUDO", "FALTA SHIRINK", "PROBLEMA NA FITA", "SEM DATA", "VOLUME BAIXO"
        };

        public string OrdemProducaoJson
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        _producaoAtual = JsonSerializer.Deserialize<ProducaoDto>(value);
                        TituloPagina = $"OP: {_producaoAtual.OrdemProducao} - {_producaoAtual.Produto}";
                        // Carrega histórico imediatamente
                        Task.Run(async () => await CarregarHistorico());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao deserializar OP: {ex}");
                        TituloPagina = "Erro ao carregar OP";
                    }
                }
            }
        }

        public PaletizacaoViewModel(IApiService apiService, IAuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
        }

        [RelayCommand]
        private async Task CarregarHistorico()
        {
            if (_producaoAtual == null) return;

            try
            {
                IsBusy = true;
                var historico = await _apiService.GetPaletesPorOPAsync(_producaoAtual.OrdemProducao);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Paletes.Clear();
                    if (historico != null && historico.Any())
                    {
                        foreach (var palete in historico.OrderByDescending(p => p.N_Palete))
                        {
                            Paletes.Add(palete);
                        }
                        TotalProduzidoGeral = historico.Max(p => p.QtdeProduzida);
                    }
                    else
                    {
                        TotalProduzidoGeral = 0;
                    }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Falha ao carregar histórico: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AdicionarPalete()
        {
            Console.WriteLine("Botão AdicionarPalete Clicado");

            // 1. Validação Básica
            if (IsBusy) return;

            if (_producaoAtual == null)
            {
                await Shell.Current.DisplayAlert("Erro", "Nenhuma Ordem de Produção carregada.", "OK");
                return;
            }

            int qtdeCaixasNoPalete = 0;

            // Lógica do Último Palete vs Palete Padrão
            if (IsUltimoPalete)
            {
                // Aqui usamos a propriedade pública QtdeUltimoPaleteText que definimos manualmente
                if (string.IsNullOrEmpty(QtdeUltimoPaleteText) || !int.TryParse(QtdeUltimoPaleteText, out qtdeCaixasNoPalete) || qtdeCaixasNoPalete <= 0)
                {
                    await Shell.Current.DisplayAlert("Atenção", "Informe uma quantidade válida de caixas para o último palete.", "OK");
                    return;
                }
            }

            // Lógica de Bloqueio
            string motivoBloqueio = null;
            if (IsBloqueado)
            {
                if (string.IsNullOrEmpty(MotivoBloqueioSelecionado))
                {
                    await Shell.Current.DisplayAlert("Atenção", "Selecione o motivo do bloqueio.", "OK");
                    return;
                }
                motivoBloqueio = MotivoBloqueioSelecionado;
            }

            try
            {
                IsBusy = true; // Trava UI

                // 2. Buscar Cadastro do Produto para obter as configurações (QtdeCaixa, Lastro, etc)
                var cadastro = await _apiService.GetCadastroPorProdutoAsync(_producaoAtual.Produto);

                if (cadastro == null)
                {
                    await Shell.Current.DisplayAlert("Erro de Cadastro", $"Não foi encontrado o cadastro para o produto: '{_producaoAtual.Produto}'. Verifique se o nome está idêntico.", "OK");
                    return;
                }

                // Se não for último palete, usa a quantidade padrão do cadastro
                if (!IsUltimoPalete)
                {
                    qtdeCaixasNoPalete = cadastro.QtdePorPalete;
                }

                // Cálculos
                int unidadesPorCaixa = cadastro.QtdeCaixa;
                int qtdeProduzidaNestePalete = qtdeCaixasNoPalete * unidadesPorCaixa;

                // Determina o número do próximo palete
                int proximoNumero = (Paletes.Any() ? Paletes.Max(p => p.N_Palete) : 0) + 1;

                // 3. Montar o DTO
                // Atenção: Certifique-se que o usuário logado não é nulo.
                string nomeUsuario = _authService.CurrentUser?.Nome ?? "UsuarioMobile";

                var novoPalete = new CreatePaleteDto
                {
                    N_Palete = proximoNumero,
                    OrdemProducao = _producaoAtual.OrdemProducao,

                    // Dados do Cadastro
                    CodigoProduto = cadastro.CodProduto,
                    Produto = _producaoAtual.Produto, // Usa o nome da OP para garantir consistência
                    Unidade = cadastro.Unidade,
                    Maquina = _producaoAtual.Maquina, // Usa a máquina da OP

                    Usuario = nomeUsuario,

                    // Quantidades - Campos Obrigatórios [Required]
                    QtdeCx = unidadesPorCaixa,       // Quantas unidades cabem numa caixa (ex: 27)
                    QtdePorPalete = qtdeCaixasNoPalete, // Quantas caixas neste palete (ex: 100)
                    QtdeProduzida = TotalProduzidoGeral + qtdeProduzidaNestePalete, // Acumulado total da OP

                    Bloqueio = motivoBloqueio
                };

                // 4. Enviar para API
                var paleteSalvo = await _apiService.SalvarPaleteAsync(novoPalete);

                if (paleteSalvo != null)
                {
                    // 5. Sucesso: Atualizar UI
                    Paletes.Insert(0, paleteSalvo);
                    TotalProduzidoGeral = paleteSalvo.QtdeProduzida;

                    // Resetar campos
                    IsUltimoPalete = false;
                    QtdeUltimoPaleteText = string.Empty;
                    IsBloqueado = false;
                    MotivoBloqueioSelecionado = null;

                    await Shell.Current.DisplayAlert("Sucesso", $"Palete {paleteSalvo.N_Palete} adicionado!", "OK");
                }
            }
            catch (HttpRequestException httpEx)
            {
                await Shell.Current.DisplayAlert("Erro de API", $"Falha ao comunicar com o servidor.\n{httpEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                // Captura genérica para erros de lógica ou nulos
                await Shell.Current.DisplayAlert("Erro Crítico", $"Ocorreu um erro inesperado:\n{ex.Message}", "OK");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                IsBusy = false; // Libera UI
            }
        }

        [RelayCommand]
        private async Task Voltar()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}