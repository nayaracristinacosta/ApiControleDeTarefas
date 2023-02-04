using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using ApiControleDeTarefas.Repositories.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ApiControleDeTarefas.Services
{
    public class RelatorioService
    {
        private readonly TarefaRepositorio _repositorio;
        public RelatorioService(TarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }


        public List<Tarefa> ObterRelatorioTarefa(RelatorioRequest filtro)
        {
            var dataInicial = DateTime.Now;
            var dataFinal = DateTime.Now;

            switch (filtro.Filtro)
            {
                case Filtro.MesPassado:
                    var mesPassado = dataInicial.AddMonths(-1);
                    dataInicial = new DateTime(mesPassado.Year, mesPassado.Month, 1);
                    dataFinal = dataInicial.AddMonths(1);
                    break;

                case Filtro.MesAtual:
                    dataInicial = new DateTime(dataInicial.Year, dataInicial.Month, 1);
                    dataFinal = dataInicial.AddMonths(1);
                    break;

                case Filtro.SemanaAtual:
                    dataInicial = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    dataFinal = dataInicial.AddDays(7);
                    break;

                case Filtro.DiaAtual:
                    dataInicial = DateTime.Today.AddHours(-24);
                    dataFinal = DateTime.Today.AddHours(+24);
                    break;
            }

            try
            {
                _repositorio.AbrirConexao();

                return _repositorio.ObterRelarotio(dataInicial, dataFinal);

            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

    }
}