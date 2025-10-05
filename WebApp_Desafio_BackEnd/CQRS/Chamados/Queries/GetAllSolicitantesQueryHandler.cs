using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetAllSolicitantesQueryHandler : IRequestHandler<GetAllSolicitantesQuery, IEnumerable<Solicitante>>
    {
        private readonly ISolicitantesDAL _dal;

        public GetAllSolicitantesQueryHandler(ISolicitantesDAL dal)
        {
            _dal = dal;
        }

        public Task<IEnumerable<Solicitante>> Handle(GetAllSolicitantesQuery request, CancellationToken cancellationToken)
        {
            var result = _dal.ListarSolicitantes();
            return Task.FromResult(result);
        }
    }
}