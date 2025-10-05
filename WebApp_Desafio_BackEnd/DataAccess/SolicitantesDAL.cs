using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class SolicitantesDAL : ISolicitantesDAL
    {
        private readonly ApplicationDbContext _context;

        public SolicitantesDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Solicitante> ObterSolicitante(int idSolicitante)
        {
            return await _context.Solicitantes.FindAsync(idSolicitante);
        }

        public async Task<IEnumerable<Solicitante>> SearchSolicitantes(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return new List<Solicitante>();
            }

            var termoUpper = termo.ToUpper();
            return await _context.Solicitantes
                                 .Where(s => s.Nome.ToUpper().Contains(termoUpper))
                                 .OrderBy(s => s.Nome)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Solicitante>> ListarSolicitantes()
        {
            return await _context.Solicitantes
                                 .OrderBy(s => s.Nome)
                                 .ToListAsync();
        }
    }
}