using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }
        public string NomeDoFuncionario { get; set; }
        public DateTime DataDeAdmissao { get; set; }
        public DateTime NascimentoDoFuncionario { get; set; }
        public string Cpf { get; set; }
        public string CelularDoFuncionario { get; set; }
        public string EmailDoFuncionario { get; set; }
        public string SenhaDoFuncionario { get; set; }
        public int Perfil { get; set; }

    }
}
