using System;

namespace WebApp_Desafio_Shared.ViewModels
{
    /// <summary>
    /// Representa os dados enviados para a API para gravar (criar/editar) um Chamado.
    /// </summary>
    public class ChamadoRequest
    {
        /// <summary>
        /// ID do Chamado (0 para um novo chamado).
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Assunto do Chamado.
        /// </summary>
        public string assunto { get; set; }

        /// <summary>
        /// ID do Solicitante do Chamado (referência à tabela de Solicitantes).
        /// </summary>
        public int idSolicitante { get; set; }

        /// <summary>
        /// ID do Departamento do Chamado.
        /// </summary>
        public int idDepartamento { get; set; }

        /// <summary>
        /// Data de Abertura do Chamado.
        /// </summary>
        public DateTime dataAbertura { get; set; }
    }
}