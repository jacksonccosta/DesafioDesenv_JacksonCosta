using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class SearchSolicitantesQueryHandler : IRequestHandler<SearchSolicitantesQuery, IEnumerable<SolicitanteDTO>>
    {
        private readonly ISolicitantesDAL _dal;

        public SearchSolicitantesQueryHandler(ISolicitantesDAL dal)
        {
            _dal = dal;
        }

        public async Task<IEnumerable<SolicitanteDTO>> Handle(SearchSolicitantesQuery request, CancellationToken cancellationToken)
        {
            var solicitantes = await _dal.SearchSolicitantes(request.TermoBusca);

            var solicitantesDTO = solicitantes.Select(s => new SolicitanteDTO
            {
                Id = s.ID.ToString(),
                Text = s.Nome
            });

            return solicitantesDTO;
        }
    }
}