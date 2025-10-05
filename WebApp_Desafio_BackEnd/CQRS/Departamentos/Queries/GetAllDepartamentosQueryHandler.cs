using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetAllDepartamentosQueryHandler : IRequestHandler<GetAllDepartamentosQuery, IEnumerable<Departamento>>
    {
        private readonly IDepartamentosDAL _dal;

        public GetAllDepartamentosQueryHandler(IDepartamentosDAL dal)
        {
            _dal = dal;
        }

        public async Task<IEnumerable<Departamento>> Handle(GetAllDepartamentosQuery request, CancellationToken cancellationToken)
        {
            return await _dal.ListarDepartamentos();
        }
    }
}