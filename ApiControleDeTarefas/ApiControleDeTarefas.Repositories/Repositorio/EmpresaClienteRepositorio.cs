using ApiControleDeTarefas.Domain.Models;
using ApiControleDeTarefas.Domain.Models.Contratos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiControleDeTarefas.Repositories.Repositorio
{
    public class EmpresaClienteRepositorio : Contexto
    {
        public EmpresaClienteRepositorio(IConfiguration configuration) : base(configuration)
        {
        }
        
        public void Inserir(EmpresaClienteRequest model)
        {
            string comandoSql = @"INSERT INTO EmpresasCliente
                                    (NomeDaEmpresa,Cnpj,EnderecoDaEmpresa,DataDeInclusaoDaEmpresa,NomeGestorDoContrato,EmailGestorDoContrato) 
                                        VALUES
                                    (@NomeDaEmpresa,@Cnpj,@EnderecoDaEmpresa,@DataDeInclusaoDaEmpresa,@NomeGestorDoContrato,@EmailGestorDoContrato);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@NomeDaEmpresa", model.NomeDaEmpresa);
                cmd.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                cmd.Parameters.AddWithValue("@EnderecoDaEmpresa", model.EnderecoDaEmpresa);
                cmd.Parameters.AddWithValue("@DataDeInclusaoDaEmpresa", model.DataDeInclusaoDaEmpresa);
                cmd.Parameters.AddWithValue("@NomeGestorDoContrato", model.NomeGestorDoContrato);
                cmd.Parameters.AddWithValue("@EmailGestorDoContrato", model.EmailGestorDoContrato);
                cmd.ExecuteNonQuery();
            }
        }

        public void Atualizar(EmpresaCliente model)
        {
            string comandoSql = @"UPDATE EmpresasCliente 
                                SET 
                                    NomeDaEmpresa = @NomeDaEmpresa,
                                    Cnpj = @Cnpj,
                                    EnderecoDaEmpresa = @EnderecoDaEmpresa,
                                    DataDeInclusaoDaEmpresa = @DataDeInclusaoDaEmpresa,                                    
                                    NomeGestorDoContrato = @NomeGestorDoContrato,
                                    EmailGestorDoContrato = @EmailGestorDoContrato
                                WHERE EmpresaClienteId = @EmpresaClienteId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", model.EmpresaClienteId);
                cmd.Parameters.AddWithValue("@NomeDaEmpresa", model.NomeDaEmpresa);
                cmd.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                cmd.Parameters.AddWithValue("@EnderecoDaEmpresa", model.EnderecoDaEmpresa);
                cmd.Parameters.AddWithValue("@DataDeInclusaoDaEmpresa", model.DataDeInclusaoDaEmpresa);
                cmd.Parameters.AddWithValue("@NomeGestorDoContrato", model.NomeGestorDoContrato);
                cmd.Parameters.AddWithValue("@EmailGestorDoContrato", model.EmailGestorDoContrato);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o EmpresaCliente de ID {model.EmpresaClienteId}");
            }
        }
        public bool SeExiste(int EmpresaClienteId)
        {
            string comandoSql = @"SELECT COUNT(EmpresaClienteId) as total FROM EmpresasCliente WHERE EmpresaClienteId = @EmpresaClienteId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", EmpresaClienteId);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }
        public EmpresaCliente? Obter(int empresaClienteId)
        {
            string comandoSql = @"SELECT EmpresaClienteId,
                                         NomeDaEmpresa,
                                         Cnpj,
                                         EnderecoDaEmpresa,   
                                         DataDeInclusaoDaEmpresa,
                                         NomeGestorDoContrato, 
                                         EmailGestorDoContrato
                                         FROM EmpresasCliente WHERE EmpresaClienteId = @EmpresaClienteId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", empresaClienteId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var EmpresaCliente = new EmpresaCliente();
                        EmpresaCliente.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        EmpresaCliente.NomeDaEmpresa = Convert.ToString(rdr["NomeDaEmpresa"]);
                        EmpresaCliente.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        EmpresaCliente.EnderecoDaEmpresa = Convert.ToString(rdr["EnderecoDaEmpresa"]);
                        EmpresaCliente.DataDeInclusaoDaEmpresa = Convert.ToDateTime(rdr["DataDeInclusaoDaEmpresa"]);
                        EmpresaCliente.NomeGestorDoContrato = Convert.ToString(rdr["NomeGestorDoContrato"]);
                        EmpresaCliente.EmailGestorDoContrato = Convert.ToString(rdr["EmailGestorDoContrato"]);
                        return EmpresaCliente;
                    }
                    else
                        return null;
                }
            }
        }
        public List<EmpresaCliente> ListarEmpresaClientes(string? nomeDaEmpresa)
        {
            string comandoSql = @"SELECT EmpresaClienteId,
                                         NomeDaEmpresa,
                                         Cnpj,
                                         EnderecoDaEmpresa,   
                                         DataDeInclusaoDaEmpresa,
                                         NomeGestorDoContrato, 
                                         EmailGestorDoContrato
                                 FROM    EmpresasCliente";

            if (!string.IsNullOrWhiteSpace(nomeDaEmpresa))
                comandoSql += " WHERE NomeDaEmpresa LIKE @NomeDaEmpresa";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                if (!string.IsNullOrWhiteSpace(nomeDaEmpresa))
                    cmd.Parameters.AddWithValue("@NomeDaEmpresa", "%" + nomeDaEmpresa + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var EmpresaClientes = new List<EmpresaCliente>();
                    while (rdr.Read())
                    {
                        var EmpresaCliente = new EmpresaCliente();
                        EmpresaCliente.EmpresaClienteId = Convert.ToInt32(rdr["EmpresaClienteId"]);
                        EmpresaCliente.NomeDaEmpresa = Convert.ToString(rdr["NomeDaEmpresa"]);
                        EmpresaCliente.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        EmpresaCliente.EnderecoDaEmpresa = Convert.ToString(rdr["EnderecoDaEmpresa"]);
                        EmpresaCliente.DataDeInclusaoDaEmpresa = Convert.ToDateTime(rdr["DataDeInclusaoDaEmpresa"]);
                        EmpresaCliente.NomeGestorDoContrato = Convert.ToString(rdr["NomeGestorDoContrato"]);
                        EmpresaCliente.EmailGestorDoContrato = Convert.ToString(rdr["EmailGestorDoContrato"]);
                        EmpresaClientes.Add(EmpresaCliente);
                    }
                    return EmpresaClientes;
                }
            }
        }
        public void Deletar(int empresaClienteId)
        {
            string comandoSql = @"DELETE FROM EmpresasCliente
                                WHERE EmpresaClienteId = @EmpresaClienteId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", empresaClienteId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o EmpresasCliente ID informado {empresaClienteId}");
            }
        }
    }
}
