using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Utils
{
    public static class ConstantUtil
    {
        public const string Gerente = "1";
        public const string Funcionario = "2";   
        public const string Geral = $"{Gerente},{Funcionario}";
    }
}
