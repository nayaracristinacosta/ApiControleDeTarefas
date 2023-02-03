﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Domain.Models
{
    public class EmpresaCliente
    {
        public int EmpresaClienteId { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string EnderecoDaEmpresa { get; set; }
        public DateTime DataDeInclusaoDaEmpresa { get; set; }
        public string NomeGestorDoContrato { get; set; }
        public string EmailGestorDoContrato { get; set; }
    }
}
