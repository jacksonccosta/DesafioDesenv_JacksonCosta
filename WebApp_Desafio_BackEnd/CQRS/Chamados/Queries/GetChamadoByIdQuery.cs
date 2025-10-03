using MediatR;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetChamadoByIdQuery : IRequest<Chamado>
    {
        public int Id { get; set; }
    }
}