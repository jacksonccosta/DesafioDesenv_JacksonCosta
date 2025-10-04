using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandHandler : IRequestHandler<GravarChamadoCommand, bool>
    {
        private readonly ChamadosDAL _chamadosDal;
        private readonly SolicitantesDAL _solicitantesDal;

        public GravarChamadoCommandHandler(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _chamadosDal = new ChamadosDAL(connectionString);
            _solicitantesDal = new SolicitantesDAL(connectionString);
        }

        public Task<bool> Handle(GravarChamadoCommand request, CancellationToken cancellationToken)
        {
            if (request.ID == 0 && request.DataAbertura.Date < DateTime.Today)
            {
                throw new ApplicationException("Não é permitido CRIAR chamados com data retroativa.");
            }

            var solicitante = _solicitantesDal.ObterSolicitante(request.IdSolicitante);
            if (solicitante == null)
            {
                throw new ApplicationException($"Solicitante com ID {request.IdSolicitante} não encontrado.");
            }

            var chamado = new Chamado
            {
                ID = request.ID,
                Assunto = request.Assunto,
                IdSolicitante = request.IdSolicitante,
                Solicitante = solicitante.Nome,
                IdDepartamento = request.IdDepartamento,
                DataAbertura = request.DataAbertura
            };

            var result = _chamadosDal.GravarChamado(chamado);
            return Task.FromResult(result);
        }
    }
}