namespace WebApp_Desafio_Shared.ViewModels
{
    /// <summary>
    /// Resposta da chamada para Departamentos (usado pela API)
    /// </summary>
    public class DepartamentoResponse
    {
        /// <summary>
        /// ID do Departamento
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Descrição do Departamento
        /// </summary>
        public string descricao { get; set; }
    }
}