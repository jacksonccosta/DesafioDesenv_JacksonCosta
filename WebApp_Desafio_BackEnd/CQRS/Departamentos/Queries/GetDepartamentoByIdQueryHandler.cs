using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetDepartamentoByIdQueryHandler : IRequestHandler<GetDepartamentoByIdQuery, Departamento>
    {
        private readonly DepartamentosDAL _dal;

        public GetDepartamentoByIdQueryHandler(IConfiguration configuration)
        {
            _dal = new DepartamentosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<Departamento> Handle(GetDepartamentoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("O ID do departamento é inválido.");

            var departamento = _dal.ObterDepartamento(request.Id);

            if (departamento == null)
                throw new ApplicationException("Departamento não encontrado.");

            return Task.FromResult(departamento);
        }
    }
}