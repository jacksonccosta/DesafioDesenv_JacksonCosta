using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetAllChamadosQueryHandler : IRequestHandler<GetAllChamadosQuery, IEnumerable<Chamado>>
    {
        private readonly IChamadosDAL _dal;

        public GetAllChamadosQueryHandler(IChamadosDAL dal)
        {
            _dal = dal;
        }

        public async Task<IEnumerable<Chamado>> Handle(GetAllChamadosQuery request, CancellationToken cancellationToken)
        {
            return await _dal.ListarChamados();
        }
    }
}