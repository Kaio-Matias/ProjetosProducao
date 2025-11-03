using Valedourado.Supervisor.Views;

namespace Valedourado.Supervisor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registra todas as rotas para garantir que a navegação programática
        // via GoToAsync funcione corretamente em todo o aplicativo.

        // Rotas para páginas que NÃO estão na TabBar
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(OpDetailPage), typeof(OpDetailPage));
        Routing.RegisterRoute(nameof(GestaoPage), typeof(GestaoPage));

        // Rotas para páginas que ESTÃO na TabBar (redundante, mas boa prática)
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(HistoricoPage), typeof(HistoricoPage));
    }
}