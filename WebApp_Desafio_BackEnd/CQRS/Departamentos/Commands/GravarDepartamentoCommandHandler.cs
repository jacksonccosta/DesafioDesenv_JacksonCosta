using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandHandler : IRequestHandler<GravarDepartamentoCommand, bool>
    {
        private readonly IDepartamentosDAL _departamentosDal;

        public GravarDepartamentoCommandHandler(IDepartamentosDAL departamentosDal)
        {
            _departamentosDal = departamentosDal ?? throw new System.ArgumentNullException(nameof(departamentosDal));
        }

        public async Task<bool> Handle(GravarDepartamentoCommand request, CancellationToken cancellationToken)
        {
            var departamento = new Departamento
            {
                ID = request.ID,
                Descricao = request.Descricao
            };

            return await _departamentosDal.GravarDepartamento(departamento);
        }
    }
}