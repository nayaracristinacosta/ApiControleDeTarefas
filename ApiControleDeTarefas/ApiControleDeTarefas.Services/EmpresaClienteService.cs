
using ApiControleDeTarefas.Domain.Models;
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
        public void Inserir(EmpresaCliente model)
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
