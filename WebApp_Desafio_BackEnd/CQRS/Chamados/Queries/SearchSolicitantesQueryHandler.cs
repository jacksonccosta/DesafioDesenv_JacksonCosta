using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class SearchSolicitantesQueryHandler : IRequestHandler<SearchSolicitantesQuery, IEnumerable<SolicitanteDTO>>
    {
        private readonly SolicitantesDAL _dal;

        public SearchSolicitantesQueryHandler(IConfiguration configuration)
        {
            _dal = new SolicitantesDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IEnumerable<SolicitanteDTO>> Handle(SearchSolicitantesQuery request, CancellationToken cancellationToken)
        {
            var solicitantes = _dal.SearchSolicitantes(request.TermoBusca);

            var solicitantesDTO = solicitantes.Select(s => new SolicitanteDTO
            {
                Id = s.ID.ToString(),
                Text = s.Nome
            });

            return Task.FromResult(solicitantesDTO);
        }
    }
}