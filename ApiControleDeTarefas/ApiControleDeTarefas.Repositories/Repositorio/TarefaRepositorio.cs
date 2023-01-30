using ApiControleDeTarefas.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Repositories.Repositorio
{
    public class TarefaRepositorio : Contexto
    {
        public TarefaRepositorio(IConfiguration configuration) : base(configuration)
        {
        }

        public void Inserir(Tarefa model)
        {
            string comandoSql = @"INSERT INTO Tarefas
                                    (FuncionarioId,EmpresaClienteId,Descricao,DataHorarioInicioTarefa,DataHorarioFimTarefa ) 
                                        VALUES
                                    (@FuncionarioId,@EmpresaClienteId,@Descricao,@DataHorarioInicioTarefa,@DataHorarioFimTarefa);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", model.FuncionarioId);
                cmd.Parameters.AddWithValue("@EmpresaClienteId", model.EmpresaClienteId);
                cmd.Parameters.AddWithValue("@DataHorarioInicioTarefa", model.DataHorarioInicioTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioFimTarefa", model.DataHorarioFimTarefa);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Tarefa model)
        {
            string comandoSql = @"UPDATE Tarefas 
                                SET 
                                    FuncionarioId = @FuncionarioId,
                                    EmpresaClienteId = @EmpresaClienteId,
                                    Descricao = @Descricao,
                                    DataHorarioInicioTarefa = @DataHorarioInicioTarefa,
                                    DataHorarioFimTarefa = @DataHorarioFimTarefa
                                WHERE TarefaId = @TarefaId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", model.FuncionarioId);
                cmd.Parameters.AddWithValue("@EmpresaClienteId", model.EmpresaClienteId);
                cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
                cmd.Parameters.AddWithValue("@DataHorarioInicioTarefa", model.DataHorarioInicioTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioFimTarefa", model.DataHorarioFimTarefa);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para a Tarefa de ID {model.TarefaId}");
            }
        }
        public bool SeExiste(int tarefaId)
        {
            string comandoSql = @"SELECT COUNT(TarefaId) as total FROM Tarefas WHERE TarefaId = @TarefaId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public Tarefa? Obter(int tarefaId)
        {
            string comandoSql = @"SELECT FuncionarioId,
                                         EmpresaClienteId,
                                         Descricao,         
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa FROM Tarefas WHERE TarefaId = @TarefaId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var Tarefa = new Tarefa();
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        return Tarefa;
                    }
                    else
                        return null;
                }
            }
        }
        public List<Tarefa> ListarTarefas(string? descricao)
        {
            string comandoSql = @"SELECT FuncionarioId,
                                         EmpresaClienteId,
                                         Descricao,         
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa     
                                 FROM    Tarefas";

            if (!string.IsNullOrWhiteSpace(descricao))
                comandoSql += " WHERE Descricao LIKE @Descricao";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                if (!string.IsNullOrWhiteSpace(descricao))
                    cmd.Parameters.AddWithValue("@Descricao", "%" + descricao + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var Tarefas = new List<Tarefa>();
                    while (rdr.Read())
                    {
                        var Tarefa = new Tarefa();
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        Tarefas.Add(Tarefa);
                    }
                    return Tarefas;
                }
            }
        }
        public void Deletar(int tarefaId)
        {
            string comandoSql = @"DELETE FROM Tarefas 
                                WHERE TarefaId = @TarefaId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o Tarefa ID informado {tarefaId}");
            }
        }
    }
}
