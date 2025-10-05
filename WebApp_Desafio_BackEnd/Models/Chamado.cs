using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp_Desafio_BackEnd.Models
{
    [Serializable]
    public class Chamado
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "O Assunto é obrigatório")]
        public string Assunto { get; set; }

        [Required]
        public int IdSolicitante { get; set; }
        public Solicitante Solicitante { get; set; }

        [Required]
        public int IdDepartamento { get; set; }
        public Departamento Departamento { get; set; }

        public DateTime DataAbertura { get; set; }

        [NotMapped]
        public string SolicitanteNome { get; set; }

        [NotMapped]
        public string DepartamentoNome { get; set; }
    }
}