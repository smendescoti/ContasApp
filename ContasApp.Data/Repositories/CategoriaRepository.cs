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
    public class CategoriaRepository
    {
        //método para consultar e retornar todas
        //as categorias cadastradas na tabela
        public List<Categoria> GetAll()
        {
            //escrevendo o comando SQL
            var query = @"
                SELECT * FROM CATEGORIA
                ORDER BY DESCRICAO
            ";

            //conectando no banco de dados do SqlServer
            using (var connection = new SqlConnection(SqlServerSettings.GetConnectionString()))
            {
                //executando a consulta e retornando o resultado
                return connection.Query<Categoria>(query).ToList();
            }
        }
    }
}
