using System;

namespace WebApp_Desafio_Shared.ViewModels
{
    /// <summary>
    /// Resposta da chamada para Chamados (usado pela API)
    /// </summary>
    public class ChamadoResponse
    {
        /// <summary>
        /// ID do Chamado
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Assunto do Chamado
        /// </summary>
        public string assunto { get; set; }

        /// <summary>
        /// Nome do Solicitante do Chamado
        /// </summary>
        public string solicitante { get; set; }

        /// <summary>
        /// ID do Departamento do Chamado
        /// </summary>
        public int idDepartamento { get; set; }

        /// <summary>
        /// Nome do Departamento do Chamado
        /// </summary>
        public string departamento { get; set; }

        /// <summary>
        /// Data de Abertura do Chamado
        /// </summary>
        public DateTime dataAbertura { get; set; }
    }
}