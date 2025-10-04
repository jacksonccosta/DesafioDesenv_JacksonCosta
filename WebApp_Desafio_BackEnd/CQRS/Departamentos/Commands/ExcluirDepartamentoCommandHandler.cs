using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class ExcluirDepartamentoCommandHandler : IRequestHandler<ExcluirDepartamentoCommand, bool>
    {
        private readonly IDepartamentosDAL _departamentoDal;

        public ExcluirDepartamentoCommandHandler(IDepartamentosDAL departamentoDal)
        {
            _departamentoDal = departamentoDal ?? throw new ArgumentNullException(nameof(departamentoDal));
        }

        public Task<bool> Handle(ExcluirDepartamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("O ID do departamento é inválido.");

            var result = _departamentoDal.ExcluirDepartamento(request.Id);
            return Task.FromResult(result);
        }
    }
}