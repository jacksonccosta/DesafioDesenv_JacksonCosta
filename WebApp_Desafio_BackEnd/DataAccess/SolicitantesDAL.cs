using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class SolicitantesDAL : BaseDAL
    {
        public SolicitantesDAL(string connectionString) : base(connectionString) { }

        public IEnumerable<Solicitante> SearchSolicitantes(string termo)
        {
            var lista = new List<Solicitante>();
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Nome FROM solicitantes WHERE Nome LIKE @termo ORDER BY Nome";
                    dbCommand.Parameters.AddWithValue("@termo", $"%{termo}%");

                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            lista.Add(new Solicitante
                            {
                                ID = dataReader.GetInt32(0),
                                Nome = dataReader.GetString(1)
                            });
                        }
                    }
                }
            }
            return lista;
        }
        public IEnumerable<Solicitante> ListarSolicitantes()
        {
            var lista = new List<Solicitante>();
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Nome FROM solicitantes ORDER BY Nome";
                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            lista.Add(new Solicitante
                            {
                                ID = dataReader.GetInt32(0),
                                Nome = dataReader.GetString(1)
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}