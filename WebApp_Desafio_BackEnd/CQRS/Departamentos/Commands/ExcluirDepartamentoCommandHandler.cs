using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.DataAccess;

namespace WebApp_Desafio_BackEnd.CQRS.Departamentos.Commands
{
    public class ExcluirDepartamentoCommandHandler : IRequestHandler<ExcluirDepartamentoCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public ExcluirDepartamentoCommandHandler(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Handle(ExcluirDepartamentoCommand request, CancellationToken cancellationToken)
        {
            var departamento = await _context.Departamentos.FindAsync(new object[] { request.Id }, cancellationToken);

            if (departamento == null)
            {
                return false;
            }

            var departamentoEmUso = await _context.Chamados.AnyAsync(c => c.IdDepartamento == request.Id, cancellationToken);
            if (departamentoEmUso)
            {
                throw new ApplicationException("Não é possível excluir um departamento que está sendo utilizado em um ou mais chamados.");
            }

            _context.Departamentos.Remove(departamento);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}