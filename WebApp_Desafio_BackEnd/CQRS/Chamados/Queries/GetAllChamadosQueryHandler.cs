using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Queries
{
    public class GetAllChamadosQueryHandler : IRequestHandler<GetAllChamadosQuery, IEnumerable<Chamado>>
    {
        private readonly ChamadosDAL _dal;

        public GetAllChamadosQueryHandler(IConfiguration configuration)
        {
            _dal = new ChamadosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<IEnumerable<Chamado>> Handle(GetAllChamadosQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_dal.ListarChamados());
        }
    }
}