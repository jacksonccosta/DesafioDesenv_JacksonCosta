using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : BaseDAL, IChamadosDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public ChamadosDAL(string connectionString) : base(connectionString) { }

        public IEnumerable<Chamado> ListarChamados()
        {
            var lstChamados = new List<Chamado>();
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT c.ID, c.Assunto, s.Nome AS Solicitante, c.IdDepartamento, d.Descricao AS Departamento, c.DataAbertura, c.IdSolicitante " +
                        "FROM chamados c " +
                        "INNER JOIN departamentos d ON c.IdDepartamento = d.ID " +
                        "LEFT JOIN solicitantes s ON c.IdSolicitante = s.ID";

                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            lstChamados.Add(MapToChamado(dataReader));
                        }
                    }
                }
            }
            return lstChamados;
        }

        public Chamado ObterChamado(int idChamado)
        {
            Chamado chamado = null;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT c.ID, c.Assunto, s.Nome AS Solicitante, c.IdDepartamento, d.Descricao AS Departamento, c.DataAbertura, c.IdSolicitante " +
                        "FROM chamados c " +
                        "INNER JOIN departamentos d ON c.IdDepartamento = d.ID " +
                        "LEFT JOIN solicitantes s ON c.IdSolicitante = s.ID " +
                        "WHERE c.ID = @idChamado";

                    dbCommand.Parameters.AddWithValue("@idChamado", idChamado);

                    using (var dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            chamado = MapToChamado(dataReader);
                        }
                    }
                }
            }
            return chamado;
        }

        public bool GravarChamado(Chamado chamado)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    if (chamado.ID == 0)
                    {
                        dbCommand.CommandText =
                            "INSERT INTO chamados (Assunto, IdSolicitante, Solicitante, IdDepartamento, DataAbertura) " +
                            "VALUES (@Assunto, @IdSolicitante, @Solicitante, @IdDepartamento, @DataAbertura)";
                    }
                    else
                    {
                        dbCommand.CommandText =
                            "UPDATE chamados SET Assunto=@Assunto, IdSolicitante=@IdSolicitante, Solicitante=@Solicitante, IdDepartamento=@IdDepartamento, DataAbertura=@DataAbertura " +
                            "WHERE ID=@ID";
                        dbCommand.Parameters.AddWithValue("@ID", chamado.ID);
                    }

                    dbCommand.Parameters.AddWithValue("@Assunto", chamado.Assunto);
                    dbCommand.Parameters.AddWithValue("@IdSolicitante", chamado.IdSolicitante);
                    dbCommand.Parameters.AddWithValue("@Solicitante", chamado.Solicitante);
                    dbCommand.Parameters.AddWithValue("@IdDepartamento", chamado.IdDepartamento);
                    dbCommand.Parameters.AddWithValue("@DataAbertura", chamado.DataAbertura.ToString("yyyy-MM-dd HH:mm:ss"));

                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return (regsAfetados > 0);
        }

        public bool ExcluirChamado(int idChamado)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SqliteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "DELETE FROM chamados WHERE ID = @idChamado";
                    dbCommand.Parameters.AddWithValue("@idChamado", idChamado);
                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return (regsAfetados > 0);
        }

        private Chamado MapToChamado(SqliteDataReader dataReader)
        {
            var chamado = new Chamado();
            chamado.ID = dataReader.GetInt32(0);
            chamado.Assunto = dataReader.GetString(1);
            chamado.Solicitante = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2);
            chamado.IdDepartamento = dataReader.GetInt32(3);
            chamado.Departamento = dataReader.GetString(4);
            chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));
            chamado.IdSolicitante = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);
            return chamado;
        }
    }
}