using ApiControleDeTarefas.Domain.Models;
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
        public void Inserir(Tarefa model)
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

    }
}
