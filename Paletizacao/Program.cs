using Paletizacao.Services;
using Paletizacao.UI;
using System;
using System.Windows.Forms;

namespace Paletizacao
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Injeção de Dependência: A instância do serviço é criada aqui
            // e passada para o primeiro formulário.
            IApiService apiService = new ApiService();
            Application.Run(new LoginForm(apiService));
        }
    }
}