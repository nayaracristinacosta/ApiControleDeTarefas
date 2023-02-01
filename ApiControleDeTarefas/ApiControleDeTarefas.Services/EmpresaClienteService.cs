
using ApiControleDeTarefas.Domain.Exceptions;
using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using ApiControleDeTarefas.Repositories.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Services
{
    public class EmpresaClienteService
    {
        private readonly EmpresaClienteRepositorio _repositorio;
        public EmpresaClienteService(EmpresaClienteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<EmpresaCliente> Listar(string? descricao)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarEmpresaClientes(descricao);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public EmpresaCliente Obter(int empresaClienteId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(empresaClienteId);
                return _repositorio.Obter(empresaClienteId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(EmpresaCliente model)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Atualizar(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Deletar(int empresaClienteId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(empresaClienteId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(EmpresaClienteRequest model)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Inserir(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        private static void ValidarModelEmpresaCliente(EmpresaCliente model, bool isUpdate = false)
        {
            #region Valida Model
            if (model is null)
                throw new ValidacaoException("O json está mal formatado, ou foi enviado vazio.");
            #endregion

            #region Valida Nome

            if (model.NomeDaEmpresa.Trim().Length < 3 || model.NomeDaEmpresa.Trim().Length > 255)
                throw new ValidacaoException("O Nome da empresa não pode estar vazio e precisa ter entre 3 a 255 caracteres.");

            if (string.IsNullOrWhiteSpace(model.NomeDaEmpresa))
                throw new ValidacaoException("O Nome do empresa é obrigatório.");

            model.NomeDaEmpresa = model.NomeDaEmpresa.Trim();

            #endregion

            var isCnpjValido = ValidaCnpj(model.Cnpj);
            if (!isCnpjValido)
                throw new ValidacaoException("O Nome do Funcionário é obrigatório.");

            if (model.EnderecoDaEmpresa.Trim().Length < 3 || model.EnderecoDaEmpresa.Trim().Length > 255)
                throw new ValidacaoException("O endereço da empresa não pode estar vazio e precisa ter entre 3 a 255 caracteres.");

            if (string.IsNullOrWhiteSpace(model.EnderecoDaEmpresa))
                throw new ValidacaoException("O endereço da empresa é obrigatório.");

            if (model.NomeGestorDoContrato.Trim().Length < 3 || model.NomeGestorDoContrato.Trim().Length > 255)
                throw new ValidacaoException("O nome do gestor do contrato não pode estar vazio e precisa ter entre 3 a 255 caracteres.");

            if (string.IsNullOrWhiteSpace(model.NomeGestorDoContrato))
                throw new ValidacaoException("O nome do gestor do contrato é obrigatório.");

            #region Valida Data de inclusão da empresa

            string dataInclusaoDaEmpresa = model.DataDeInclusaoDaEmpresa.ToString();
            bool isValidDataInclusaoDaEmpresa = FuncionarioService.ValidaData(dataInclusaoDaEmpresa);

            if (!isValidDataInclusaoDaEmpresa)
                throw new ValidacaoException("A Data de inclusão da empresa está incorreta.");
            #endregion

            #region Valida Email do Gestor

            var isEmailValido = FuncionarioService.ValidaEmail(model.EmailGestorDoContrato);
            if(!isEmailValido)
                throw new ValidacaoException("O E-mail informado está incorreto ou não é válido.");

            #endregion
        }

        public int EmpresaClienteId { get; set; }
        public string NomeDaEmpresa { get; set; }
        public string Cnpj { get; set; }
        public string EnderecoDaEmpresa { get; set; }
        public DateTime DataDeInclusaoDaEmpresa { get; set; }
        public string NomeGestorDoContrato { get; set; }
        public string EmailGestorDoContrato { get; set; }

        public static bool ValidaCnpj(string cnpj)

        {

            string CNPJ = cnpj.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            int[] digitos, soma, resultado;

            int nrDig;

            string ftmt;

            bool[] CNPJOk;

            ftmt = "6543298765432";

            digitos = new int[14];

            soma = new int[2];

            soma[0] = 0;

            soma[1] = 0;

            resultado = new int[2];

            resultado[0] = 0;

            resultado[1] = 0;

            CNPJOk = new bool[2];

            CNPJOk[0] = false;

            CNPJOk[1] = false;

            try

            {
                for (nrDig = 0; nrDig < 14; nrDig++)

                {

                    digitos[nrDig] = int.Parse(

                        CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)

                        soma[0] += (digitos[nrDig] *

                          int.Parse(ftmt.Substring(

                          nrDig + 1, 1)));

                    if (nrDig <= 12)

                        soma[1] += (digitos[nrDig] *

                          int.Parse(ftmt.Substring(

                          nrDig, 1)));

                }

                for (nrDig = 0; nrDig < 2; nrDig++)

                {

                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (

                         resultado[nrDig] == 1))

                        CNPJOk[nrDig] = (

                        digitos[12 + nrDig] == 0);

                    else

                        CNPJOk[nrDig] = (

                        digitos[12 + nrDig] == (

                        11 - resultado[nrDig]));

                }

                return (CNPJOk[0] && CNPJOk[1]);

            }

            catch

            {
                return false;

            }

        }

    }
}
