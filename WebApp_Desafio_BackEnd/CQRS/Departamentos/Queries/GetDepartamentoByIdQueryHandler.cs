using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Queries
{
    public class GetDepartamentoByIdQueryHandler : IRequestHandler<GetDepartamentoByIdQuery, Departamento>
    {
        private readonly IDepartamentosDAL _dal;

        public GetDepartamentoByIdQueryHandler(IDepartamentosDAL dal)
        {
            _dal = dal;
        }

        public async Task<Departamento> Handle(GetDepartamentoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("O ID do departamento é inválido.");

            var departamento = await _dal.ObterDepartamento(request.Id);

            if (departamento == null)
                throw new ApplicationException("Departamento não encontrado.");

            return departamento;
        }
    }
}