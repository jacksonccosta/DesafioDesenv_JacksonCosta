using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommandHandler : IRequestHandler<ExcluirChamadoCommand, bool>
    {
        private readonly IChamadosDAL _chamadosDal;

        public ExcluirChamadoCommandHandler(IChamadosDAL chamadosDal)
        {
            _chamadosDal = chamadosDal ?? throw new ArgumentNullException(nameof(chamadosDal));
        }

        public Task<bool> Handle(ExcluirChamadoCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new System.ArgumentException("O ID do chamado é inválido.");

            var result = _chamadosDal.ExcluirChamado(request.Id);
            return Task.FromResult(result);
        }
    }
}