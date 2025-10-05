using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class GravarDepartamentoCommandHandler : IRequestHandler<GravarDepartamentoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public GravarDepartamentoCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(GravarDepartamentoCommand request, CancellationToken cancellationToken)
        {

            if (request.ID > 0)
            {
                var departamentoExistente = await _context.Departamentos.FindAsync(new object[] { request.ID }, cancellationToken);
                if (departamentoExistente == null)
                {
                    throw new ApplicationException("Departamento não encontrado para atualização.");
                }
                departamentoExistente.Descricao = request.Descricao;
            }
            else
            {
                var novoDepartamento = new Departamento
                {
                    Descricao = request.Descricao
                };
                _context.Departamentos.Add(novoDepartamento);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}