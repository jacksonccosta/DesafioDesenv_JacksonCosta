using MediatR;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}