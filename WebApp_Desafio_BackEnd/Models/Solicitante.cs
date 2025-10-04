using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Desafio_BackEnd.Models
{
    [Serializable]
    public class Solicitante
    {
        [Key]
        public int ID { get; set; }

        public string Nome { get; set; }
    }
}