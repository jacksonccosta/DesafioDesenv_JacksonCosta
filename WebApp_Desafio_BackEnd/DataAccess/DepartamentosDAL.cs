using System.Collections.Generic;
using System.Data.SQLite;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : BaseDAL
    {
        public DepartamentosDAL(string connectionString) : base(connectionString) { }

        public IEnumerable<Departamento> ListarDepartamentos()
        {
            var lstDepartamentos = new List<Departamento>();
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Descricao FROM departamentos ORDER BY Descricao";
                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            lstDepartamentos.Add(MapToDepartamento(dataReader));
                        }
                    }
                }
            }
            return lstDepartamentos;
        }

        /// <summary>
        /// Obtém um departamento específico pelo seu ID.
        /// </summary>
        /// <param name="id">O ID do departamento a ser buscado.</param>
        /// <returns>O objeto Departamento ou null se não for encontrado.</returns>
        public Departamento ObterDepartamento(int id)
        {
            Departamento departamento = null;
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Descricao FROM departamentos WHERE ID = @ID";
                    // Utiliza parâmetros para prevenir SQL Injection
                    dbCommand.Parameters.AddWithValue("@ID", id);
                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            departamento = MapToDepartamento(dataReader);
                        }
                    }
                }
            }
            return departamento;
        }

        /// <summary>
        /// Grava (insere um novo ou atualiza um existente) um departamento no banco de dados.
        /// </summary>
        /// <param name="departamento">O objeto Departamento com os dados a serem gravados.</param>
        /// <returns>True se a operação for bem-sucedida, False caso contrário.</returns>
        public bool GravarDepartamento(Departamento departamento)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    // Se o ID for 0, é uma inserção (INSERT)
                    if (departamento.ID == 0)
                    {
                        dbCommand.CommandText = "INSERT INTO departamentos (Descricao) VALUES (@Descricao)";
                    }
                    // Se o ID for maior que 0, é uma atualização (UPDATE)
                    else
                    {
                        dbCommand.CommandText = "UPDATE departamentos SET Descricao=@Descricao WHERE ID=@ID";
                        dbCommand.Parameters.AddWithValue("@ID", departamento.ID);
                    }

                    // Adiciona o parâmetro de Descrição, que é comum a ambas as operações
                    dbCommand.Parameters.AddWithValue("@Descricao", departamento.Descricao);
                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            // Retorna true se uma ou mais linhas foram afetadas
            return regsAfetados > 0;
        }

        /// <summary>
        /// Mapeia um registro do SQLiteDataReader para um objeto Departamento.
        /// </summary>
        private Departamento MapToDepartamento(SQLiteDataReader dataReader)
        {
            var depto = new Departamento();
            if (!dataReader.IsDBNull(0)) depto.ID = dataReader.GetInt32(0);
            if (!dataReader.IsDBNull(1)) depto.Descricao = dataReader.GetString(1);
            return depto;
        }
    }
}