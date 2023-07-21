using ContasApp.Data.Entities;
using ContasApp.Data.Settings;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContasApp.Data.Repositories
{
    public class ContaRepository
    {
        //método para cadastrar uma conta na tabela
        public void Add(Conta conta)
        {
            //escrevendo o comando SQL
            var query = @"
                INSERT INTO CONTA(ID, NOME, DATA, VALOR, TIPO, OBSERVACOES, CATEGORIAID, USUARIOID)
                VALUES(@Id, @Nome, @Data, @Valor, @Tipo, @Observacoes, @CategoriaID, @UsuarioId)
            ";

            //conectando no banco de dados do sqlserver
            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                connection.Execute(query, conta);
            }
        }

        //método para atualizar uma conta na tabela
        public void Update(Conta conta)
        {
            //escrevendo o comando SQL
            var query = @"
                UPDATE CONTA 
                SET
                    NOME = @Nome,
                    DATA = @Data,
                    VALOR = @Valor,
                    TIPO = @Tipo,
                    OBSERVACOES = @Observacoes,
                    CATEGORIAID = @CategoriaId
                WHERE
                    ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                connection.Execute(query, conta);
            }
        }

        //método para excluir uma conta na tabela
        public void Delete(Conta conta)
        {
            //escrevendo o comando SQL
            var query = @"
                DELETE FROM CONTA 
                WHERE ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                connection.Execute(query, conta);
            }
        }

        //método para consultar contas por datas e usuário
        public List<Conta> GetByDatasAndUsuario(DateTime dataInicio, DateTime dataFim, Guid usuarioId)
        {
            //escrevendo o comando SQL
            var query = @"
                SELECT * FROM CONTA co
                INNER JOIN CATEGORIA ca
                ON co.CATEGORIAID = ca.ID
                WHERE co.USUARIOID = @UsuarioId 
                  AND co.DATA BETWEEN @DataInicio AND @DataFim
                ORDER BY co.DATA DESC
            ";

            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                return connection.Query(query,
                    (Conta co, Categoria ca) =>
                    {
                        co.Categoria = ca;
                        return co;
                    },
                    new { @UsuarioId = usuarioId, @DataInicio = dataInicio, @DataFim = dataFim }, 
                    splitOn: "CategoriaId")
                    .ToList();
            }
        }

        //método para consultar 1 conta no banco de dados através do ID
        public Conta? GetById(Guid id)
        {
            //escrevendo o comando SQL
            var query = @"
                SELECT * FROM CONTA
                WHERE ID = @Id
            ";

            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                return connection.Query<Conta>(query, new { @Id = id }).FirstOrDefault();
            }
        }

    }
}
