namespace Valedourado.Shared.Dtos
{
    public class DashboardDto
    {
        public int OpsAbertas { get; set; }
        public int TotalProduzidoHoje { get; set; }
        public int TotalPerdidoHoje { get; set; }
        // Futuramente podemos adicionar mais KPIs, como Eficiência Média
    }
}