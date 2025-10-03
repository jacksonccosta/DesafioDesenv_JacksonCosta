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
        private readonly ChamadosDAL _dal;

        public GravarChamadoCommandHandler(IConfiguration configuration)
        {
            _dal = new ChamadosDAL(configuration.GetConnectionString("DefaultConnection"));
        }

        public Task<bool> Handle(GravarChamadoCommand request, CancellationToken cancellationToken)
        {
            // DESAFIO EXTRA (Requisito 1): Validação de Regra de Negócio
            if (request.DataAbertura.Date < DateTime.Today)
            {
                throw new ApplicationException("Não é permitido criar ou editar chamados com data retroativa.");
            }

            var chamado = new Chamado
            {
                ID = request.ID,
                Assunto = request.Assunto,
                Solicitante = request.Solicitante,
                IdDepartamento = request.IdDepartamento,
                DataAbertura = request.DataAbertura
            };

            var result = _dal.GravarChamado(chamado);
            return Task.FromResult(result);
        }
    }
}