using MediatR;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class ExcluirDepartamentoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}