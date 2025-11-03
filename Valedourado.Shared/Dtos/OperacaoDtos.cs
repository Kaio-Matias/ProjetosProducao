// Arquivo: Valedourado.Shared/DTOs/OperacaoDtos.cs

using System;
using System.Collections.Generic;

namespace Valedourado.Shared.Dtos
{
    // DTO para criar um novo detalhamento de OP
    public class CreateDetalhamentoOpDto
    {
        public int OrdemProducao { get; set; }
        public string? Operador { get; set; }
        public string? Turno { get; set; }
        public int EmbProcessadas { get; set; }
        public int EmbProduzidas { get; set; }
        public int EmbPerdidas { get; set; }
    }

    // DTO para registrar perdas
    public class PerdasDto
    {
        public int OrdemProducao { get; set; }
        public string? Motivo { get; set; }
        public int Quantidade { get; set; }
        public string? Operador { get; set; }
    }

    // DTO para registrar paradas de eficiência
    public class EficienciaDto
    {
        public int OrdemProducao { get; set; }
        public string? Motivo { get; set; }
        public TimeSpan Tempo { get; set; }
        public string? Operador { get; set; }
    }
}