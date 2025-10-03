using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommandHandler : IRequestHandler<ExcluirChamadoCommand, bool>
    {
        private readonly ChamadosDAL _dal;

        public ExcluirChamadoCommandHandler(IConfiguration configuration)
        {
            _dal = new ChamadosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<bool> Handle(ExcluirChamadoCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new System.ArgumentException("O ID do chamado é inválido.");

            var result = _dal.ExcluirChamado(request.Id);
            return Task.FromResult(result);
        }
    }
}