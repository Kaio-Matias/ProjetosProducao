using Valedourado.Supervisor.Views;

namespace Valedourado.Supervisor
{
    public partial class App : Application
    {
        public App(LoginPage loginPage) // Recebe a LoginPage já construída com todas as dependências
        {
            InitializeComponent();

            // Define a LoginPage como a página principal inicial.
            MainPage = loginPage;
        }
    }
}