using Valedourado.Shared.Dtos;
using Paletizacao.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paletizacao.UI
{
    public partial class PaletizacaoForm : Form
    {
        private readonly IApiService _apiService;
        private readonly ProducaoDto _ordemProducaoAtual;
        private readonly UsuarioDto _usuarioLogado;
        private readonly BindingList<PaleteDto> _paletesExibidos = new BindingList<PaleteDto>();
        private int _totalProduzidoGeral = 0;

        public PaletizacaoForm(ProducaoDto op, UsuarioDto usuario, IApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            _ordemProducaoAtual = op;
            _usuarioLogado = usuario;

            this.Load += async (s, e) => await PaletizacaoForm_Load(s, e);
            this.chkBloqueio.CheckedChanged += ChkBloqueio_CheckedChanged;
            this.chkUltimoPalete.CheckedChanged += ChkUltimoPalete_CheckedChanged;
            this.btnVoltar.Click += (s, e) => this.Close();
        }

        private async Task PaletizacaoForm_Load(object sender, EventArgs e)
        {
            dgvPaletes.DataSource = _paletesExibidos;
            ConfigureDataGridView();

            chklistBloqueio.Items.AddRange(new object[] {
                "FALTA CANUDO", "FALTA SHIRINK", "PROBLEMA NA FITA", "SEM DATA", "VOLUME BAIXO"
            });

            lblInfoOp.Text = $"OP: {_ordemProducaoAtual.OrdemProducao} - {_ordemProducaoAtual.Produto}";

            await LoadHistoricoPaletes();
            txtCodigoDeBarras.Focus();
        }

        private async void TxtCodigoDeBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            e.Handled = true;
            string codBarra = txtCodigoDeBarras.Text.Trim();
            if (string.IsNullOrWhiteSpace(codBarra)) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                CadastroDto cadastroDoProduto = await _apiService.GetCadastroPorCodBarraAsync(codBarra);

                if (cadastroDoProduto == null)
                {
                    MessageBox.Show("Código de barras não encontrado no cadastro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int qtdeCaixasFinal = cadastroDoProduto.QtdeCaixa;
                int qtdePorPaleteFinal = chkUltimoPalete.Checked ? (int)numQtdeUltimoPalete.Value : cadastroDoProduto.QtdePorPalete;
                int qtdeProduzidaNestePalete = qtdeCaixasFinal * qtdePorPaleteFinal;

                int proximoNumeroPalete = (_paletesExibidos.LastOrDefault()?.N_Palete ?? 0) + 1;

                string motivosBloqueio = chkBloqueio.Checked ? string.Join(", ", chklistBloqueio.CheckedItems.Cast<string>()) : null;

                var paleteParaSalvar = new CreatePaleteDto
                {
                    N_Palete = proximoNumeroPalete,
                    OrdemProducao = _ordemProducaoAtual.OrdemProducao,
                    CodigoProduto = cadastroDoProduto.CodProduto,
                    Produto = cadastroDoProduto.Produto,
                    Unidade = cadastroDoProduto.Unidade,
                    Maquina = cadastroDoProduto.Maquina,
                    Usuario = _usuarioLogado.Nome,
                    QtdeCx = qtdeCaixasFinal,
                    QtdePorPalete = qtdePorPaleteFinal,
                    QtdeProduzida = _totalProduzidoGeral + qtdeProduzidaNestePalete,
                    Bloqueio = motivosBloqueio
                };

                PaleteDto paleteSalvo = await _apiService.SalvarPaleteAsync(paleteParaSalvar);

                _paletesExibidos.Add(paleteSalvo);
                _totalProduzidoGeral = paleteSalvo.QtdeProduzida;

                AtualizarUI(paleteSalvo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao registrar palete: {ex.Message}", "Erro de Processamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await LoadHistoricoPaletes();
            }
            finally
            {
                txtCodigoDeBarras.Focus();
                this.Cursor = Cursors.Default;
            }
        }

        private void AtualizarUI(PaleteDto ultimoPalete)
        {
            txtProduto.Text = ultimoPalete.Produto;
            lblTotalProduzido.Text = $"Total Produzido: {_totalProduzidoGeral}";

            if (dgvPaletes.RowCount > 0)
                dgvPaletes.FirstDisplayedScrollingRowIndex = dgvPaletes.RowCount - 1;

            txtCodigoDeBarras.Clear();
            chkUltimoPalete.Checked = false;
            chkBloqueio.Checked = false;
        }

        private void ConfigureDataGridView()
        {
            dgvPaletes.AutoGenerateColumns = false;
            dgvPaletes.Columns.Clear();

            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "N_Palete", HeaderText = "Nº Palete", FillWeight = 10 });
            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Produto", HeaderText = "Produto", FillWeight = 40 });
            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "QtdeCx", HeaderText = "Qtde Caixas", FillWeight = 15 });
            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "QtdePorPalete", HeaderText = "Lastro", FillWeight = 10 });
            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "QtdeProduzida", HeaderText = "Total Acumulado", FillWeight = 20 });
            dgvPaletes.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Bloqueio", HeaderText = "Bloqueio", FillWeight = 25 });
        }

        private async Task LoadHistoricoPaletes()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var historico = await _apiService.GetPaletesPorOPAsync(_ordemProducaoAtual.OrdemProducao);
                _paletesExibidos.Clear();
                _totalProduzidoGeral = 0;

                if (historico != null && historico.Any())
                {
                    foreach (var palete in historico.OrderBy(p => p.N_Palete))
                    {
                        _paletesExibidos.Add(palete);
                    }
                    _totalProduzidoGeral = historico.Max(p => p.QtdeProduzida);
                }
                lblTotalProduzido.Text = $"Total Produzido: {_totalProduzidoGeral}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar histórico: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void ChkBloqueio_CheckedChanged(object sender, EventArgs e)
        {
            chklistBloqueio.Visible = chkBloqueio.Checked;
            if (chkBloqueio.Checked)
            {
                for (int i = 0; i < chklistBloqueio.Items.Count; i++)
                    chklistBloqueio.SetItemChecked(i, false);
            }
        }

        private void ChkUltimoPalete_CheckedChanged(object sender, EventArgs e)
        {
            numQtdeUltimoPalete.Enabled = chkUltimoPalete.Checked;
            if (chkUltimoPalete.Checked)
            {
                numQtdeUltimoPalete.Value = 0;
                numQtdeUltimoPalete.Focus();
            }
            else
            {
                txtCodigoDeBarras.Focus();
            }
        }
    }
}