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
    public class TarefaService
    {
        private readonly TarefaRepositorio _repositorio;
        public TarefaService(TarefaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<Tarefa> Listar(string? descricao)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.ListarTarefas(descricao);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public Tarefa Obter(int tarefaId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(tarefaId);
                return _repositorio.Obter(tarefaId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(Tarefa model)
        {
            try
            {
                _repositorio.AbrirConexao();

                CalculaHorasGastas(model);

                _repositorio.Atualizar(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Deletar(int tarefaId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(tarefaId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(TarefaRequest model)
        {
            try
            {
                _repositorio.AbrirConexao();
                ValidaSeExisteFuncionarioId(model.FuncionarioId);
                ValidaSeExisteEmpresaId(model.EmpresaClienteId);
                ValidarModelTarefa(model);
                _repositorio.Inserir(model);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }

        public Tarefa CalculaHorasGastas(Tarefa model)
        {
            var tempoInicial = model.DataHorarioInicioTarefa.TimeOfDay;
            var tempoFinal = model.DataHorarioFimTarefa.TimeOfDay;
            model.TempoTotalGastoTarefa = Convert.ToString(tempoFinal - tempoInicial);
            return model;
        }

        #region Valida Model Tarefa
        private static void ValidarModelTarefa(TarefaRequest model, bool isUpdate = false)      
        {
            #region Valida Model
            if (model is null)
                throw new ValidacaoException("O json está mal formatado, ou foi enviado vazio.");
            #endregion

            #region Valida Descrição
            if (model.Descricao.Trim().Length < 3 || model.Descricao.Trim().Length > 255)
                throw new ValidacaoException("A Descrição não pode estar vazia e precisa ter entre 3 a 255 caracteres.");
            #endregion
        }
        #endregion

        #region Valida FuncionarioId
        public void ValidaSeExisteFuncionarioId(int funcionarioId)
        {
            bool isFuncionarioIdValid = _repositorio.SeExisteFuncionarioId(funcionarioId);

            if (!isFuncionarioIdValid)
                throw new ValidacaoException("Id do Funcionário não localizado no Sistema!");

        }
        #endregion

        #region Valida EmpresaId
        public void ValidaSeExisteEmpresaId(int empresaId)
        {
            bool isEmpresaIdValid = _repositorio.SeExisteEmpresaId(empresaId);

            if (!isEmpresaIdValid)
                throw new ValidacaoException("Id da Empresa não localizado no Sistema!");

        }
        #endregion
    }
}

