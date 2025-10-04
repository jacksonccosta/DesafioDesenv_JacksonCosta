using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebApp_Desafio_Shared.ViewModels
{
    [DataContract]
    public class SolicitanteViewModel
    {
        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Nome")]
        [DataMember(Name = "Nome")]
        public string Nome { get; set; }
    }
}