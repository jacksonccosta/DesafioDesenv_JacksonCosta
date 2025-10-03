using MediatR;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetDepartamentoByIdQuery : IRequest<Departamento>
    {
        public int Id { get; set; }
    }
}