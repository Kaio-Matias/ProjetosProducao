using Valedourado.Shared.Dtos;
using Paletizacao.Services;
using System;
using System.Windows.Forms;

namespace Paletizacao.UI
{
    public partial class LoginForm : Form
    {
        private readonly IApiService _apiService;

        public LoginForm(IApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            this.Resize += (s, e) => { if (pnlMain != null) pnlMain.Location = new System.Drawing.Point((this.ClientSize.Width - pnlMain.Width) / 2, (this.ClientSize.Height - pnlMain.Height) / 2); };
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMatricula.Text, out int matricula))
            {
                MessageBox.Show("Por favor, insira uma matrícula numérica válida.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnEntrar.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                var loginRequest = new LoginRequestDto { Matricula = matricula };
                UsuarioDto usuario = await _apiService.LoginAsync(loginRequest);

                if (!"paletizador".Equals(usuario?.Cargo, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Seu cargo não permite o acesso a este aplicativo.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Hide();
                var opSelectionForm = new OpSelectionForm(usuario, _apiService);
                opSelectionForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnEntrar.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void linkLabelRegistreSe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = new RegisterForm(_apiService);
            registerForm.ShowDialog();
        }
    }
}