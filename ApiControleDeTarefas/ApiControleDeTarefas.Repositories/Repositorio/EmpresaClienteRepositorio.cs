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
    public class EmpresaClienteRepositorio : Contexto
    {
        public EmpresaClienteRepositorio(IConfiguration configuration) : base(configuration)
        {
        }
        
        public void Inserir(EmpresaCliente model)
        {
            string comandoSql = @"INSERT INTO EmpresasCliente
                                    (NomeDaEmpresa,Cnpj,EnderecoDaEmpresa,NomeGestorDoContrato) 
                                        VALUES
                                    (@NomeDaEmpresa,@Cnpj,@EnderecoDaEmpresa,@NomeGestorDoContrato);";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@NomeDaEmpresa", model.NomeDaEmpresa);
                cmd.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                cmd.Parameters.AddWithValue("@EnderecoDaEmpresa", model.EnderecoDaEmpresa);
                cmd.Parameters.AddWithValue("@NomeGestorDoContrato", model.NomeGestorDoContrato);
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
                                    NomeGestorDoContrato = @NomeGestorDoContrato
                                WHERE EmpresaClienteId = @EmpresaClienteId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@NomeDaEmpresa", model.NomeDaEmpresa);
                cmd.Parameters.AddWithValue("@Cnpj", model.Cnpj);
                cmd.Parameters.AddWithValue("@EnderecoDaEmpresa", model.EnderecoDaEmpresa);
                cmd.Parameters.AddWithValue("@NomeGestorDoContrato", model.NomeGestorDoContrato);
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
            string comandoSql = @"SELECT NomeDaEmpresa,
                                         Cnpj,
                                         EnderecoDaEmpresa,         
                                         NomeGestorDoContrato   
                                         Perfil FROM EmpresasCliente WHERE EmpresaClienteId = @EmpresaClienteId";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", empresaClienteId);

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var EmpresaCliente = new EmpresaCliente();
                        EmpresaCliente.NomeDaEmpresa = Convert.ToString(rdr["NomeDaEmpresa"]);
                        EmpresaCliente.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        EmpresaCliente.EnderecoDaEmpresa = Convert.ToString(rdr["EnderecoDaEmpresa"]);
                        EmpresaCliente.NomeGestorDoContrato = Convert.ToString(rdr["NomeGestorDoContrato"]);
                        return EmpresaCliente;
                    }
                    else
                        return null;
                }
            }
        }
        public List<EmpresaCliente> ListarEmpresaClientes(string? nomeDaEmpresa)
        {
            string comandoSql = @"SELECT NomeDoEmpresaCliente,
                                         Cnpj,
                                         EnderecoDaEmpresa,         
                                         NomeGestorDoContrato
                                 FROM    EmpresaClientes";

            if (!string.IsNullOrWhiteSpace(nomeDaEmpresa))
                comandoSql += " WHERE NomeDaEmpresa LIKE @NomeDaEmpresa";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                if (!string.IsNullOrWhiteSpace(nomeDaEmpresa))
                    cmd.Parameters.AddWithValue("@NomeDoEmpresaCliente", "%" + nomeDaEmpresa + "%");

                using (var rdr = cmd.ExecuteReader())
                {
                    var EmpresaClientes = new List<EmpresaCliente>();
                    while (rdr.Read())
                    {
                        var EmpresaCliente = new EmpresaCliente();
                        EmpresaCliente.NomeDaEmpresa = Convert.ToString(rdr["NomeDaEmpresa"]);
                        EmpresaCliente.Cnpj = Convert.ToString(rdr["Cnpj"]);
                        EmpresaCliente.EnderecoDaEmpresa = Convert.ToString(rdr["EnderecoDaEmpresa"]);
                        EmpresaCliente.NomeGestorDoContrato = Convert.ToString(rdr["NomeGestorDoContrato"]);
                        EmpresaClientes.Add(EmpresaCliente);
                    }
                    return EmpresaClientes;
                }
            }
        }
        public void Deletar(int empresaClienteId)
        {
            string comandoSql = @"DELETE FROM EmpresaClientes 
                                WHERE EmpresaClienteId = @EmpresaClienteId;";

            using (var cmd = new SqlCommand(comandoSql, _conn))
            {
                cmd.Parameters.AddWithValue("@EmpresaClienteId", empresaClienteId);
                if (cmd.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"Nenhum registro afetado para o EmpresaCliente ID informado {empresaClienteId}");
            }
        }
    }
}
