using System;
using System.Collections.Generic;
using System.Data.SQLite;

using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : BaseDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public ChamadosDAL(string connectionString) : base(connectionString) { }

        public IEnumerable<Chamado> ListarChamados()
        {
            IList<Chamado> lstChamados = new List<Chamado>();

            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT c.ID, c.Assunto, c.Solicitante, c.IdDepartamento, d.Descricao AS Departamento, c.DataAbertura " +
                        "FROM chamados c " +
                        "INNER JOIN departamentos d ON c.IdDepartamento = d.ID";

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
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT c.ID, c.Assunto, c.Solicitante, c.IdDepartamento, d.Descricao AS Departamento, c.DataAbertura " +
                        "FROM chamados c " +
                        "INNER JOIN departamentos d ON c.IdDepartamento = d.ID " +
                        "WHERE c.ID = @idChamado";

                    // CORREÇÃO DE SQL INJECTION
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
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    if (chamado.ID == 0)
                    {
                        dbCommand.CommandText =
                            "INSERT INTO chamados (Assunto, Solicitante, IdDepartamento, DataAbertura) " +
                            "VALUES (@Assunto, @Solicitante, @IdDepartamento, @DataAbertura)";
                    }
                    else
                    {
                        dbCommand.CommandText =
                            "UPDATE chamados SET Assunto=@Assunto, Solicitante=@Solicitante, IdDepartamento=@IdDepartamento, DataAbertura=@DataAbertura " +
                            "WHERE ID=@ID";
                        dbCommand.Parameters.AddWithValue("@ID", chamado.ID);
                    }

                    dbCommand.Parameters.AddWithValue("@Assunto", chamado.Assunto);
                    dbCommand.Parameters.AddWithValue("@Solicitante", chamado.Solicitante);
                    dbCommand.Parameters.AddWithValue("@IdDepartamento", chamado.IdDepartamento);
                    dbCommand.Parameters.AddWithValue("@DataAbertura", chamado.DataAbertura.ToString(ANSI_DATE_FORMAT));

                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return (regsAfetados > 0);
        }

        public bool ExcluirChamado(int idChamado)
        {
            int regsAfetados = 0;
            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "DELETE FROM chamados WHERE ID = @idChamado";

                    // CORREÇÃO DE SQL INJECTION
                    dbCommand.Parameters.AddWithValue("@idChamado", idChamado);

                    regsAfetados = dbCommand.ExecuteNonQuery();
                }
            }
            return (regsAfetados > 0);
        }

        private Chamado MapToChamado(SQLiteDataReader dataReader)
        {
            var chamado = new Chamado();
            if (!dataReader.IsDBNull(0)) chamado.ID = dataReader.GetInt32(0);
            if (!dataReader.IsDBNull(1)) chamado.Assunto = dataReader.GetString(1);
            if (!dataReader.IsDBNull(2)) chamado.Solicitante = dataReader.GetString(2);
            if (!dataReader.IsDBNull(3)) chamado.IdDepartamento = dataReader.GetInt32(3);
            if (!dataReader.IsDBNull(4)) chamado.Departamento = dataReader.GetString(4);
            if (!dataReader.IsDBNull(5)) chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));
            return chamado;
        }
    }
}