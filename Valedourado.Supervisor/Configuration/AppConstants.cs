namespace Valedourado.Supervisor.Configuration
{
    public static class AppConstants
    {
        public const string SupervisorRole = "Supervisor";

        public static class ApiEndpoints
        {
            // --- Usuario ---
            public const string Login = "/api/Usuario/login";
            public const string Register = "/api/Usuario/register";

            // --- Relatorio ---
            public const string Dashboard = "/api/Relatorio/dashboard";
            public const string RelatorioCompletoOp = "/api/Relatorio/op/{0}"; // {0} = ordemProducao
            public const string RelatorioPdf = "/api/Relatorio/op/{0}/pdf";   // {0} = ordemProducao

            // --- Producoes ---
            public const string ProducoesAbertas = "/api/Producoes/abertas";
            public const string ProducoesFechadasPorData = "/api/Producoes/closed/bydate?startDate={0}&endDate={1}"; // {0} = startDate, {1} = endDate
            public const string FecharProducao = "/api/Producoes/{0}/fechar";    // {0} = ordemProducao
            public const string CancelarProducao = "/api/Producoes/{0}/cancelar";  // {0} = ordemProducao
        }
    }
}