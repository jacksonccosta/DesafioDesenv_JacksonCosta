using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : BaseDAL, IDepartamentosDAL
    {
        public DepartamentosDAL(string connectionString) : base(connectionString) { }

        public IEnumerable<Departamento> ListarDepartamentos()
        {
            var lstDepartamentos = new List<Departamento>();
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
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

        public Departamento ObterDepartamento(int id)
        {
            Departamento departamento = null;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Descricao FROM departamentos WHERE ID = @ID";
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

        public bool GravarDepartamento(Departamento departamento)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    if (departamento.ID == 0)
                    {
                        dbCommand.CommandText = "INSERT INTO departamentos (Descricao) VALUES (@Descricao)";
                    }
                    else
                    {
                        dbCommand.CommandText = "UPDATE departamentos SET Descricao=@Descricao WHERE ID=@ID";
                        dbCommand.Parameters.AddWithValue("@ID", departamento.ID);
                    }

                    dbCommand.Parameters.AddWithValue("@Descricao", departamento.Descricao);
                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return regsAfetados > 0;
        }

        public bool ExcluirDepartamento(int id)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "DELETE FROM departamentos WHERE ID = @ID";
                    dbCommand.Parameters.AddWithValue("@ID", id);
                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return regsAfetados > 0;
        }

        private Departamento MapToDepartamento(SqliteDataReader dataReader)
        {
            var depto = new Departamento();
            depto.ID = dataReader.GetInt32(0);
            depto.Descricao = dataReader.GetString(1);
            return depto;
        }
    }
}