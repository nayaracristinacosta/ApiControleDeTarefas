namespace ApiControleDeTarefas.Domain.Models.Contratos
{
    public class RelatorioRequest
    {
        public Filtro Filtro { get; set; }

    }
    public enum Filtro
    {
        DiaAtual = 1,
        SemanaAtual = 2,
        MesAtual = 3,
        MesPassado = 4
    }
}
