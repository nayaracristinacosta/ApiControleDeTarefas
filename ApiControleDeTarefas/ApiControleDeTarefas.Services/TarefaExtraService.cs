using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Repositories.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Services
{
    public class TarefaExtraService
    {
        private readonly TarefaExtraRepositorio _repositorio;
        public TarefaExtraService(TarefaExtraRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public List<TarefaExtra> Listar(string? descricao)
        {
            try
            {
                _repositorio.AbrirConexao();
                return _repositorio.Listar(descricao);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public TarefaExtra Obter(int tarefaExtraId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.SeExiste(tarefaExtraId);
                return _repositorio.Obter(tarefaExtraId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Atualizar(TarefaExtra model)
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
        public void Deletar(int tarefaExtraId)
        {
            try
            {
                _repositorio.AbrirConexao();
                _repositorio.Deletar(tarefaExtraId);
            }
            finally
            {
                _repositorio.FecharConexao();
            }
        }
        public void Inserir(TarefaExtra model)
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
