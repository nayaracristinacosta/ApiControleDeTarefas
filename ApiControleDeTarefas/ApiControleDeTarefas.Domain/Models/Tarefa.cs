using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Models
{
    public class Tarefa
    {
        public int TarefaId { get; set; }
        public int FuncionarioId { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHorarioInicioTarefa { get; set; }
        public DateTime DataHorarioFimTarefa { get; set; }
        public int EmpresaClienteId { get; set; }

    }
}
