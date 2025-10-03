using MediatR;
using System.Collections.Generic;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    // Utilizarei um DTO (Data Transfer Object) simples para não trafegar o modelo de domínio completo.
    public class SolicitanteDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class SearchSolicitantesQuery : IRequest<IEnumerable<SolicitanteDTO>>
    {
        public string TermoBusca { get; set; }
    }
}