using ApiControleDeTarefas.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Repositories.Repositorio
{
    public class TarefaExtraRepositorio : Contexto
    {
        public TarefaExtraRepositorio(IConfiguration configuration) : base(configuration)
        {
        }
        public void Inserir(TarefaExtra model)
        {
            string comandoSql = @"INSERT INTO TarefasExtra
                                    (TarefaId,Descricao,TempoTarefaExtra ) 
                                        VALUES
                                    (@TarefaId,@Descricao,@TempoTarefaExtra);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", model.TarefaId);
                cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
                cmd.Parameters.AddWithValue("@TempoTarefaExtra", model.TempoTarefaExtra);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(TarefaExtra model)
        {
            string comandoSql = @"UPDATE TarefasExtra 
                                SET 
                                    TarefaId = @TarefaId,
                                    Descricao = @Descricao,
                                    TempoTarefaExtra = @TempoTarefaExtra
                                WHERE TarefaExtraId = @TarefaExtraId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", model.TarefaId);
                cmd.Parameters.AddWithValue("@Descricao", model.Descricao);
                cmd.Parameters.AddWithValue("@TempoTarefaExtra", model.TempoTarefaExtra);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para a Tarefa de ID {model.TarefaExtraId}");
            }
        }
        public bool SeExiste(int tarefaExtraId)
        {
            string comandoSql = @"SELECT COUNT(TarefaExtraId) as total FROM TarefasExtra WHERE TarefaExtraId = @TarefaExtraId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaExtraId", tarefaExtraId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public TarefaExtra? Obter(int tarefaExtraId)
        {
            string comandoSql = @"SELECT TarefaId,
                                         Descricao,
                                         TempoTarefaExtra
                                FROM TarefasExtra WHERE TarefaExtraId = @TarefaExtraId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaExtraId", tarefaExtraId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var tarefaExtra = new TarefaExtra();
                        tarefaExtra.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        tarefaExtra.Descricao = Convert.ToString(rdr["Descricao"]);
                        tarefaExtra.TempoTarefaExtra = Convert.ToDateTime(rdr["TempoTarefaExtra"]);
                        return tarefaExtra;
                    }
                    else
                        return null;
                }
            }
        }
        public List<TarefaExtra> Listar(string? descricao)
        {
            string comandoSql = @"SELECT TarefaId,
                                         Descricao,
                                         TempoTarefaExtra  
                                 FROM    TarefasExtra";

            if (!string.IsNullOrWhiteSpace(descricao))
                comandoSql += " WHERE Descricao LIKE @Descricao";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                if (!string.IsNullOrWhiteSpace(descricao))
                    cmd.Parameters.AddWithValue("@Descricao", "%" + descricao + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var TarefasExtra = new List<TarefaExtra>();
                    while (rdr.Read())
                    {
                        var tarefaExtra = new TarefaExtra();
                        tarefaExtra.TarefaId = Convert.ToInt32(rdr["TarefaId"]);
                        tarefaExtra.Descricao = Convert.ToString(rdr["Descricao"]);
                        tarefaExtra.TempoTarefaExtra = Convert.ToDateTime(rdr["TempoTarefaExtra"]);
                        TarefasExtra.Add(tarefaExtra);
                    }
                    return TarefasExtra;
                }
            }
        }
        public void Deletar(int tarefaExtraId)
        {
            string comandoSql = @"DELETE FROM TarefasExtra 
                                WHERE TarefaExtraId = @TarefaExtraId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@TarefaId", tarefaExtraId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o Tarefa ID informado {tarefaExtraId}");
            }
        }
    }
}
