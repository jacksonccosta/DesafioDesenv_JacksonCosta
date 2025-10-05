using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class GravarChamadoCommandHandler : IRequestHandler<GravarChamadoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public GravarChamadoCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(GravarChamadoCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    if (request.ID == 0 && request.DataAbertura.Date < DateTime.Today)
                        throw new ApplicationException("Não é permitido CRIAR chamados com data retroativa.");

                    var solicitanteExiste = await _context.Solicitantes.AnyAsync(s => s.ID == request.IdSolicitante, cancellationToken);
                    if (!solicitanteExiste)
                        throw new ApplicationException($"Solicitante com ID {request.IdSolicitante} não encontrado.");

                    var departamentoExiste = await _context.Departamentos.AnyAsync(d => d.ID == request.IdDepartamento, cancellationToken);
                    if (!departamentoExiste)
                        throw new ApplicationException($"Departamento com ID {request.IdDepartamento} não encontrado.");

                    if (request.ID > 0)
                    {
                        var chamadoExistente = await _context.Chamados.FindAsync(new object[] { request.ID }, cancellationToken);
                        if (chamadoExistente == null)
                            throw new ApplicationException("Chamado não encontrado para atualização.");

                        chamadoExistente.Assunto = request.Assunto;
                        chamadoExistente.IdSolicitante = request.IdSolicitante;
                        chamadoExistente.IdDepartamento = request.IdDepartamento;
                        chamadoExistente.DataAbertura = request.DataAbertura;
                    }
                    else
                    {
                        var novoChamado = new Chamado
                        {
                            Assunto = request.Assunto,
                            IdSolicitante = request.IdSolicitante,
                            IdDepartamento = request.IdDepartamento,
                            DataAbertura = request.DataAbertura
                        };
                        _context.Chamados.Add(novoChamado);
                    }

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    transaction.Commit();

                    return success;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}