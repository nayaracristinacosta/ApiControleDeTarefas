using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Models
{
    public class TarefaExtra
    {
        public int TarefaExtraId { get; set; }
        public int TarefaId { get; set; }
        public string Descricao { get; set; }
        public DateTime TempoTarefaExtra { get; set; }

    }
}
