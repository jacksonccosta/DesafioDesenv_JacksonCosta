using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Chamados.Commands
{
    public class ExcluirChamadoCommandHandler : IRequestHandler<ExcluirChamadoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ExcluirChamadoCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(ExcluirChamadoCommand request, CancellationToken cancellationToken)
        {

            var chamado = await _context.Chamados.FindAsync(new object[] { request.Id }, cancellationToken);

            if (chamado == null)
            {
                return false;
            }

            _context.Chamados.Remove(chamado);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}