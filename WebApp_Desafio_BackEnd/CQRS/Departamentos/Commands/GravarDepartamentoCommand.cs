using MediatR;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommand : IRequest<bool>
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
    }
}