using Valedourado.Shared.Dtos;
using Paletizacao.Services;
using System;
using System.Windows.Forms;

namespace Paletizacao.UI
{
    public partial class RegisterForm : Form
    {
        private readonly IApiService _apiService;

        public RegisterForm(IApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            this.btnVoltar.Click += (s, e) => this.Close();
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtCargo.Text) ||
                !int.TryParse(txtMatricula.Text, out int matricula))
            {
                MessageBox.Show("Todos os campos são obrigatórios e a matrícula deve ser um número.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var novoUsuario = new CreateUsuarioDto
            {
                Nome = txtNome.Text,
                Cargo = txtCargo.Text,
                Matricula = matricula
            };

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnRegistrar.Enabled = false;

                await _apiService.RegisterAsync(novoUsuario);

                MessageBox.Show("Usuário registrado com sucesso! Por favor, faça o login.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Não foi possível registrar o usuário: {ex.Message}", "Erro de Registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRegistrar.Enabled = true;
            }
        }
    }
}