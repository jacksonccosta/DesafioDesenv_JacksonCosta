using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetChamadoByIdQueryHandler : IRequestHandler<GetChamadoByIdQuery, Chamado>
    {
        private readonly IChamadosDAL _chamadosDal;

        public GetChamadoByIdQueryHandler(IChamadosDAL chamadosDal)
        {
            _chamadosDal = chamadosDal ?? throw new ArgumentNullException(nameof(chamadosDal));
        }

        public Task<Chamado> Handle(GetChamadoByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("O ID do chamado é inválido.");

            var chamado = _chamadosDal.ObterChamado(request.Id);

            if (chamado == null)
                throw new ApplicationException("Chamado não encontrado.");

            return Task.FromResult(chamado);
        }
    }
}