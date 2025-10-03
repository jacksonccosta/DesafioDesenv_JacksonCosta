using MediatR;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetAllChamadosQuery : IRequest<IEnumerable<Chamado>> { }
}