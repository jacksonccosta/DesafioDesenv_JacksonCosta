using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace WebApp_Desafio_Shared.ViewModels
{
    [DataContract]
    public class ChamadoViewModel
    {
        private CultureInfo ptBR = new CultureInfo("pt-BR");

        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Assunto")]
        [DataMember(Name = "Assunto")]
        public string Assunto { get; set; }

        [Display(Name = "Solicitante")]
        [DataMember(Name = "Solicitante")]
        public string Solicitante { get; set; }

        [Display(Name = "IdSolicitante")]
        [DataMember(Name = "IdSolicitante")]
        [Range(1, int.MaxValue, ErrorMessage = "O Solicitante é obrigatório.")]
        public int IdSolicitante { get; set; }

        [Display(Name = "IdDepartamento")]
        [DataMember(Name = "IdDepartamento")]
        public int IdDepartamento { get; set; }

        [Display(Name = "Departamento")]
        [DataMember(Name = "Departamento")]
        public string Departamento { get; set; }

        [Display(Name = "Data de Abertura")]
        [DataMember(Name = "DataAbertura")]
        public DateTime DataAbertura { get; set; }

        [DataMember(Name = "DataAberturaWrapper")]
        public string DataAberturaWrapper => DataAbertura.ToString("d", ptBR);
    }
}