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
        private readonly DepartamentosDAL _dal;

        public ExcluirDepartamentoCommandHandler(IConfiguration configuration)
        {
            _dal = new DepartamentosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<bool> Handle(ExcluirDepartamentoCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("O ID do departamento é inválido.");

            var result = _dal.ExcluirDepartamento(request.Id);
            return Task.FromResult(result);
        }
    }
}