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
        private readonly ChamadosDAL _dal;

        public SearchSolicitantesQueryHandler(IConfiguration configuration)
        {
            _dal = new ChamadosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IEnumerable<SolicitanteDTO>> Handle(SearchSolicitantesQuery request, CancellationToken cancellationToken)
        {
            var todosChamados = _dal.ListarChamados();

            // Lógica para buscar solicitantes distintos.
            // Em uma aplicação real, isso viria de uma tabela de "Usuários" ou "Solicitantes".
            // Aqui, estou simulando a busca a partir dos chamados já existentes.
            var solicitantes = todosChamados
                .Where(c => !string.IsNullOrEmpty(c.Solicitante) && c.Solicitante.ToLower().Contains(request.TermoBusca.ToLower()))
                .Select(c => c.Solicitante)
                .Distinct()
                .Select(nome => new SolicitanteDTO { Id = nome, Text = nome })
                .OrderBy(s => s.Text);

            return Task.FromResult(solicitantes.AsEnumerable());
        }
    }
}