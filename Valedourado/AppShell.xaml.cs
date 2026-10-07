using Valedourado.Views;

namespace Valedourado
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AberturaPage), typeof(AberturaPage));
            Routing.RegisterRoute(nameof(OpSelectionPage), typeof(OpSelectionPage));
            Routing.RegisterRoute(nameof(DetalhamentoPage), typeof(DetalhamentoPage));
            Routing.RegisterRoute(nameof(LossOpSelectionPage), typeof(LossOpSelectionPage));
            Routing.RegisterRoute(nameof(RecordLossesPage), typeof(RecordLossesPage));
            Routing.RegisterRoute(nameof(EfficiencyOpSelectionPage), typeof(EfficiencyOpSelectionPage));
            Routing.RegisterRoute(nameof(RecordEfficiencyPage), typeof(RecordEfficiencyPage));
            Routing.RegisterRoute(nameof(PaletizacaoPage), typeof(PaletizacaoPage)); 
            Routing.RegisterRoute(nameof(PaletizacaoOpSelectionPage), typeof(PaletizacaoOpSelectionPage));
            Routing.RegisterRoute(nameof(PaletizacaoPage), typeof(PaletizacaoPage));

        }
    }
}
