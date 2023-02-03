using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Models.Contratos
{
    public class TarefaRequest
    {
        public int FuncionarioId { get; set; }
        public int EmpresaClienteId { get; set; }
        public string AssuntoTarefa { get; set; }
        public string Descricao { get; set; }
        public TipoDaTarefa TipoDaTarefa { get; set; }
        public DateTime DataHorarioInicioTarefa { get; set; }
        public DateTime DataHorarioFimTarefa { get; set; }
        
    }
}
