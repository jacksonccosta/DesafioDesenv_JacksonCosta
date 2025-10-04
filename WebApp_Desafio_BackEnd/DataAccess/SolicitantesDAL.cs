using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class SolicitantesDAL : BaseDAL, ISolicitantesDAL
    {
        public SolicitantesDAL(string connectionString) : base(connectionString) { }

        public Solicitante ObterSolicitante(int idSolicitante)
        {
            Solicitante solicitante = null;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT ID, Nome FROM solicitantes WHERE ID = @idSolicitante";
                    dbCommand.Parameters.AddWithValue("@idSolicitante", idSolicitante);

                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            solicitante = new Solicitante
                            {
                                ID = dataReader.GetInt32(0),
                                Nome = dataReader.GetString(1)
                            };
                        }
                    }
                }
            }
            return solicitante;
        }

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