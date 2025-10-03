using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetAllDepartamentosQueryHandler : IRequestHandler<GetAllDepartamentosQuery, IEnumerable<Departamento>>
    {
        private readonly DepartamentosDAL _dal;

        public GetAllDepartamentosQueryHandler(IConfiguration configuration)
        {
            _dal = new DepartamentosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IEnumerable<Departamento>> Handle(GetAllDepartamentosQuery request, CancellationToken cancellationToken)
        {
            var result = _dal.ListarDepartamentos();
            return Task.FromResult(result);
        }
    }
}