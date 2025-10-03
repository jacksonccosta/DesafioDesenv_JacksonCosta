using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandHandler : IRequestHandler<GravarDepartamentoCommand, bool>
    {
        private readonly DepartamentosDAL _dal;

        public GravarDepartamentoCommandHandler(IConfiguration configuration)
        {
            _dal = new DepartamentosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<bool> Handle(GravarDepartamentoCommand request, CancellationToken cancellationToken)
        {
            var departamento = new Departamento
            {
                ID = request.ID,
                Descricao = request.Descricao
            };

            var result = _dal.GravarDepartamento(departamento);
            return Task.FromResult(result);
        }
    }
}