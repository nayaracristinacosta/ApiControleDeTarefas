﻿using ApiControleDeTarefas.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Repositories.Repositorio
{
    public class FuncionarioRepositorio : Contexto
    {
        public FuncionarioRepositorio(IConfiguration configuration) : base(configuration)
        {
        }
        public Funcionario? ObterFuncionarioPorCredenciais(string email, string senha)
        {
            string comandoSql = @"SELECT EmailDoFuncionario, NomeDoFuncionario, Perfil FROM Funcionarios 
                                    WHERE u.EmailFuncionario = @EmailFuncionario AND u.SenhaFuncionario = @SenhaFuncionario";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmailFuncionario", email);
                cmd.Parameters.AddWithValue("@SenhaFuncionario", senha);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Funcionario()
                        {
                            NomeDoFuncionario = rdr["NomeDoFuncionario"].ToString(),
                            EmailDoFuncionario = rdr["EmailFuncionario"].ToString(),
                            Perfil = Convert.ToInt32(rdr["CargoId"])
                        };
                    }
                    else
                        return null;
                }
            }
        }
        public void Inserir(Funcionario model)
        {
            string comandoSql = @"INSERT INTO Funcionarios
                                    (NomeDoFuncionario,NascimentoDoFuncionario,DataDeAdmissao,Cpf,CelularDoFuncionario,EmailDoFuncionario,SenhaDoFuncionario,Perfil) 
                                        VALUES
                                    (@NomeDoFuncionario,@NascimentoDoFuncionario,@DataDeAdmissao,@Cpf,@CelularDoFuncionario,@EmailDoFuncionario,@SenhaDoFuncionario,@Perfil);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@NomeDoFuncionario", model.NomeDoFuncionario);
                cmd.Parameters.AddWithValue("@NascimentoDoFuncionario", model.NascimentoDoFuncionario);
                cmd.Parameters.AddWithValue("@DataDeAdmissao", model.DataDeAdmissao);
                cmd.Parameters.AddWithValue("@Cpf", model.Cpf);
                cmd.Parameters.AddWithValue("@CelularDoFuncionario", model.CelularDoFuncionario);
                cmd.Parameters.AddWithValue("@EmailDoFuncionario", model.EmailDoFuncionario);
                cmd.Parameters.AddWithValue("@SenhaDoFuncionario", model.SenhaDoFuncionario);
                cmd.Parameters.AddWithValue("@Perfil", model.Perfil);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(Funcionario model)
        {
            string comandoSql = @"UPDATE Funcionarios 
                                SET 
                                    NomeDoFuncionario = @NomeDoFuncionario,
                                    NascimentoDoFuncionario = @NascimentoDoFuncionario,
                                    DataDeAdmissao = @DataDeAdmissao,
                                    Cpf = @Cpf,
                                    CelularDoFuncionario = @CelularDoFuncionario,
                                    EmailDoFuncionario = @EmailDoFuncionario,
                                    SenhaDoFuncionario = @SenhaDoFuncionario,
                                    Perfil = @Perfil
                                WHERE FuncionarioId = @FuncionarioId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@NomeDoFuncionario", model.NomeDoFuncionario);
                cmd.Parameters.AddWithValue("@NascimentoDoFuncionario", model.NascimentoDoFuncionario);
                cmd.Parameters.AddWithValue("@DataDeAdmissao", model.DataDeAdmissao);
                cmd.Parameters.AddWithValue("@Cpf", model.Cpf);
                cmd.Parameters.AddWithValue("@CelularDoFuncionario", model.CelularDoFuncionario);
                cmd.Parameters.AddWithValue("@EmailDoFuncionario", model.EmailDoFuncionario);
                cmd.Parameters.AddWithValue("@SenhaDoFuncionario", model.SenhaDoFuncionario);
                cmd.Parameters.AddWithValue("@Perfil", model.Perfil);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o Funcionario de ID {model.FuncionarioId}");
            }
        }
        public bool SeExiste(int FuncionarioId)
        {
            string comandoSql = @"SELECT COUNT(FuncionarioId) as total FROM Funcionarios WHERE FuncionarioId = @FuncionarioId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", FuncionarioId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public Funcionario? Obter(int FuncionarioId)
        {
            string comandoSql = @"SELECT NomeDoFuncionario,
                                         NascimentoDoFuncionario,
                                         DataDeAdmissao,         
                                         Cpf,    
                                         CelularDoFuncionario,
                                         EmailDoFuncionario,
                                         SenhaDoFuncionario, 
                                         Perfil FROM Funcionarios WHERE FuncionarioId = @FuncionarioId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", FuncionarioId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var Funcionario = new Funcionario();
                        Funcionario.NomeDoFuncionario = Convert.ToString(rdr["NomeDoFuncionario"]);
                        Funcionario.NascimentoDoFuncionario = Convert.ToDateTime(rdr["NascimentoFuncionario"]); ;
                        Funcionario.DataDeAdmissao = Convert.ToDateTime(rdr["DataDeAdmissao"]);
                        Funcionario.Cpf = Convert.ToString(rdr["Cpf"]);
                        Funcionario.CelularDoFuncionario = Convert.ToString(rdr["CelularDoFuncionario"]);
                        Funcionario.EmailDoFuncionario = Convert.ToString(rdr["EmailDoFuncionario"]);
                        Funcionario.SenhaDoFuncionario = Convert.ToString(rdr["SenhaDoFuncionario"]);
                        Funcionario.Perfil = Convert.ToInt32(rdr["Perfil"]);
                        return Funcionario;
                    }
                    else
                        return null;
                }
            }
        }
        public List<Funcionario> ListarFuncionarios(string? nomeDoFuncionario)
        {
            string comandoSql = @"SELECT NomeDoFuncionario,
                                         NascimentoDoFuncionario,
                                         DataDeAdmissao,         
                                         Cpf,    
                                         CelularDoFuncionario,
                                         EmailDoFuncionario,
                                         SenhaDoFuncionario, 
                                         Perfil FROM Funcionarios";

            if (!string.IsNullOrWhiteSpace(nomeDoFuncionario))
                comandoSql += " WHERE NomeDoFuncionario LIKE @NomeDoFuncionario";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                if (!string.IsNullOrWhiteSpace(nomeDoFuncionario))
                    cmd.Parameters.AddWithValue("@nomeDoFuncionario", "%" + nomeDoFuncionario + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var Funcionarios = new List<Funcionario>();
                    while (rdr.Read())
                    {
                        var Funcionario = new Funcionario();
                        Funcionario.NomeDoFuncionario = Convert.ToString(rdr["NomeDoFuncionario"]);
                        Funcionario.NascimentoDoFuncionario = Convert.ToDateTime(rdr["NascimentoFuncionario"]); ;
                        Funcionario.DataDeAdmissao = Convert.ToDateTime(rdr["DataDeAdmissao"]);
                        Funcionario.Cpf = Convert.ToString(rdr["Cpf"]);
                        Funcionario.CelularDoFuncionario = Convert.ToString(rdr["CelularDoFuncionario"]);
                        Funcionario.EmailDoFuncionario = Convert.ToString(rdr["EmailDoFuncionario"]);
                        Funcionario.SenhaDoFuncionario = Convert.ToString(rdr["SenhaDoFuncionario"]);
                        Funcionario.Perfil = Convert.ToInt32(rdr["Perfil"]);
                        Funcionarios.Add(Funcionario);
                    }
                    return Funcionarios;
                }
            }
        }
        public void Deletar(int FuncionarioId)
        {
            string comandoSql = @"DELETE FROM Funcionarios 
                                WHERE FuncionarioId = @FuncionarioId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId", FuncionarioId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o Funcionario ID informado {FuncionarioId}");
            }
        }
    }
}