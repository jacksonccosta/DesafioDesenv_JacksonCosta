using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandHandler : IRequestHandler<GravarChamadoCommand, bool>
    {
        private readonly IChamadosDAL _chamadosDal;
        private readonly ISolicitantesDAL _solicitantesDal;
        private readonly IDepartamentosDAL _departamentosDal;

        public GravarChamadoCommandHandler(IChamadosDAL chamadosDal, ISolicitantesDAL solicitantesDal, IDepartamentosDAL departamentosDal)
        {
            _chamadosDal = chamadosDal ?? throw new ArgumentNullException(nameof(chamadosDal));
            _solicitantesDal = solicitantesDal ?? throw new ArgumentNullException(nameof(solicitantesDal));
            _departamentosDal = departamentosDal ?? throw new ArgumentNullException(nameof(departamentosDal));
        }

        public async Task<bool> Handle(GravarChamadoCommand request, CancellationToken cancellationToken)
        {
            if (request.IdSolicitante <= 0)
            {
                throw new ApplicationException("O Solicitante é obrigatório.");
            }
            if (request.IdDepartamento <= 0)
            {
                throw new ApplicationException("O Departamento é obrigatório.");
            }
            if (request.ID == 0 && request.DataAbertura.Date < DateTime.Today)
            {
                throw new ApplicationException("Não é permitido CRIAR chamados com data retroativa.");
            }

            var solicitanteExiste = await _solicitantesDal.ObterSolicitante(request.IdSolicitante);
            if (solicitanteExiste == null)
            {
                throw new ApplicationException($"Solicitante com ID {request.IdSolicitante} não encontrado.");
            }

            var departamentoExiste = await _departamentosDal.ObterDepartamento(request.IdDepartamento);
            if (departamentoExiste == null)
            {
                throw new ApplicationException($"Departamento com ID {request.IdDepartamento} não encontrado.");
            }

            var chamado = new Chamado
            {
                ID = request.ID,
                Assunto = request.Assunto,
                IdSolicitante = request.IdSolicitante,
                IdDepartamento = request.IdDepartamento,
                DataAbertura = request.DataAbertura
            };

            return await _chamadosDal.GravarChamado(chamado);
        }
    }
}