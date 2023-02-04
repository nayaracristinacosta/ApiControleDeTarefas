using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
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

        public void Inserir(TarefaRequest model)
        {
            string comandoSql = @"INSERT INTO Tarefas
                                    (FuncionarioId,EmpresaClienteId,AssuntoTarefa,Descricao,TipoDaTarefa,DataHorarioInicioTarefa,DataHorarioFimTarefa ) 
                                        VALUES
                                    (@FuncionarioId,@EmpresaClienteId,@AssuntoTarefa,@Descricao,@TipoDaTarefa,@DataHorarioInicioTarefa, @DataHorarioFimTarefa);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", model.FuncionarioId);
                cmd.Parameters.AddWithValue("@EmpresaClienteId", model.EmpresaClienteId);
                cmd.Parameters.AddWithValue("@AssuntoTarefa", model.AssuntoTarefa);
                cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
                cmd.Parameters.AddWithValue("@TipoDaTarefa", model.TipoDaTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioInicioTarefa", model.DataHorarioInicioTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioFimTarefa", model.DataHorarioInicioTarefa);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Tarefa model)
        {
            string comandoSql = @"UPDATE Tarefas 
                                SET 
                                    FuncionarioId = @FuncionarioId,
                                    EmpresaClienteId = @EmpresaClienteId,
                                    AssuntoTarefa = @AssuntoTarefa,
                                    Descricao = @Descricao,
                                    TipoDaTarefa = @TipoDaTarefa,
                                    DataHorarioInicioTarefa = @DataHorarioInicioTarefa,                                    
                                    DataHorarioFimTarefa = @DataHorarioFimTarefa,
                                    TempoTotalGastoTarefa = @TempoTotalGastoTarefa
                                WHERE TarefaId = @TarefaId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", model.TarefaId);
                cmd.Parameters.AddWithValue("@FuncionarioId", model.FuncionarioId);
                cmd.Parameters.AddWithValue("@EmpresaClienteId", model.EmpresaClienteId);
       {
}
         cmd.Parameters.AddWithValue("@AssuntoTarefa", model.AssuntoTarefa);
                cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
                cmd.Parameters.AddWithValue("@TipoDaTarefa", model.TipoDaTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioInicioTarefa", model.DataHorarioInicioTarefa);
                cmd.Parameters.AddWithValue("@DataHorarioFimTarefa", model.DataHorarioFimTarefa);
                cmd.Parameters.AddWithValue("@TempoTotalGastoTarefa", model.TempoTotalGastoTarefa);
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
        public bool SeExisteFuncionarioId(int funcionarioId)
        {
            string comandoSql = @"SELECT COUNT(FuncionarioId) as total FROM Funcionarios WHERE FuncionarioId = @FuncionarioId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", funcionarioId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool SeExisteEmpresaId(int empresaId)
        {
            string comandoSql = @"SELECT COUNT(EmpresaClienteId) as total FROM EmpresasCliente WHERE EmpresaClienteId = @EmpresaClienteId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", empresaId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public Tarefa? Obter(int tarefaId)
        {
            string comandoSql = @"SELECT TarefaId,
                                         FuncionarioId,
                                         EmpresaClienteId,
                                         AssuntoTarefa,
                                         Descricao,
                                         TipoDaTarefa,
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa,
                                         TempoTotalGastoTarefa FROM Tarefas WHERE TarefaId = @TarefaId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var Tarefa = new Tarefa();
                        Tarefa.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.AssuntoTarefa = Convert.ToString(rdr["AssuntoTarefa"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.TipoDaTarefa = (TipoDaTarefa)Convert.ToInt32(rdr["TipoDaTarefa"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        Tarefa.TempoTotalGastoTarefa = Convert.ToString(rdr["TempoTotalGastoTarefa"]);
                        return Tarefa;
                    }
                    else
                        return null;
                }
            }
        }
        public List<Tarefa> ListarTarefas(string? descricao)
        {
            string comandoSql = @"SELECT TarefaId,
                                         FuncionarioId,
                                         EmpresaClienteId,
                                         AssuntoTarefa,                                            
                                         Descricao,   
                                         TipoDaTarefa,
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa,
                                         TempoTotalGastoTarefa
                                         FROM Tarefas";

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
                        Tarefa.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.AssuntoTarefa = Convert.ToString(rdr["AssuntoTarefa"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.TipoDaTarefa = (TipoDaTarefa)Convert.ToInt32(rdr["TipoDaTarefa"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        Tarefa.TempoTotalGastoTarefa = Convert.ToString(rdr["TempoTotalGastoTarefa"]);
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


        public List<Tarefa> ObterRelarotio(DateTime dataInicial, DateTime dataFinal)
        {
            string comandoSql = @"SELECT TarefaId,
                                         FuncionarioId,
                                         EmpresaClienteId,
                                         AssuntoTarefa,
                                         Descricao,
                                         TipoDaTarefa,
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa,
                                         TempoTotalGastoTarefa FROM Tarefas WHERE DataHorarioInicioTarefa >=  @dataInicial and DataHorarioInicioTarefa <= @dataFinal ";


            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@dataInicial", dataInicial);
                cmd.Parameters.AddWithValue("@dataFinal", dataFinal);

                using (var rdr = cmd.ExecuteReader())
                {
                    var Tarefas = new List<Tarefa>();
                    while (rdr.Read())
                    {
                        var Tarefa = new Tarefa();
                        Tarefa.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.AssuntoTarefa = Convert.ToString(rdr["AssuntoTarefa"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.TipoDaTarefa = (TipoDaTarefa)Convert.ToInt32(rdr["TipoDaTarefa"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        Tarefa.TempoTotalGastoTarefa = Convert.ToString(rdr["TempoTotalGastoTarefa"]);
                        Tarefas.Add(Tarefa);
                    }
                    return Tarefas;
                }
            }
        }

        public Tarefa? Obtedr(int tarefaId)
        {
            string comandoSql = @"SELECT TarefaId,
                                         FuncionarioId,
                                         EmpresaClienteId,
                                         AssuntoTarefa,
                                         Descricao,
                                         TipoDaTarefa,
                                         DataHorarioInicioTarefa,   
                                         DataHorarioFimTarefa,
                                         TempoTotalGastoTarefa FROM Tarefas WHERE TarefaId = @TarefaId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var Tarefa = new Tarefa();
                        Tarefa.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        Tarefa.FuncionarioId = Convert.ToInt32(rdr["FuncionarioId"]);
                        Tarefa.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        Tarefa.AssuntoTarefa = Convert.ToString(rdr["AssuntoTarefa"]);
                        Tarefa.Descricao = Convert.ToString(rdr["Descricao"]);
                        Tarefa.TipoDaTarefa = (TipoDaTarefa)Convert.ToInt32(rdr["TipoDaTarefa"]);
                        Tarefa.DataHorarioInicioTarefa = Convert.ToDateTime(rdr["DataHorarioInicioTarefa"]);
                        Tarefa.DataHorarioFimTarefa = Convert.ToDateTime(rdr["DataHorarioFimTarefa"]);
                        Tarefa.TempoTotalGastoTarefa = Convert.ToString(rdr["TempoTotalGastoTarefa"]);
                        return Tarefa;
                    }
                    else
                        return null;
                }
            }
        }
    }
}
