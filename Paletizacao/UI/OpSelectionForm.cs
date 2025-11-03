using Valedourado.Shared.Dtos;
using Paletizacao.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paletizacao.UI
{
    public partial class OpSelectionForm : Form
    {
        private readonly IApiService _apiService;
        private readonly UsuarioDto _usuarioLogado;

        public OpSelectionForm(UsuarioDto usuario, IApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            _usuarioLogado = usuario;

            this.lblWelcome.Text = $"Bem-vindo, {_usuarioLogado.Nome}!";

            this.Load += async (s, e) => await LoadOpsAbertas();
            this.btnAtualizar.Click += async (s, e) => await LoadOpsAbertas();
            this.dgvOpsAbertas.CellDoubleClick += dgvOpsAbertas_CellAction;
            this.btnSair.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private async Task LoadOpsAbertas()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnAtualizar.Enabled = false;
                var ops = await _apiService.GetOpenProducoesAsync();
                dgvOpsAbertas.DataSource = ops;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar Ordens de Produção: {ex.Message}", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnAtualizar.Enabled = true;
            }
        }

        private void dgvOpsAbertas_CellAction(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvOpsAbertas.Rows[e.RowIndex].DataBoundItem is ProducaoDto selectedOp)
            {
                this.Hide();
                var paletizacaoForm = new PaletizacaoForm(selectedOp, _usuarioLogado, _apiService);
                paletizacaoForm.ShowDialog();
                this.Show();
                // Opcional: Atualizar a lista após fechar o form de paletização
                // Task.Run(async () => await LoadOpsAbertas());
            }
        }
    }
}