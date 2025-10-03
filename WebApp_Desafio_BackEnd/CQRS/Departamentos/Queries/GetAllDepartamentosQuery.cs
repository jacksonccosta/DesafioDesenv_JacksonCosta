using MediatR;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetAllDepartamentosQuery : IRequest<IEnumerable<Departamento>>
    {
    }
}