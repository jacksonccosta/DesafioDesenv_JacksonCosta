using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : IChamadosDAL
    {
        private readonly ApplicationDbContext _context;

        public ChamadosDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chamado>> ListarChamados()
        {
            return await _context.Chamados
                .AsNoTracking()
                .Include(c => c.Solicitante)
                .Include(c => c.Departamento)
                .OrderByDescending(c => c.ID)
                .ToListAsync();
        }

        public async Task<Chamado> ObterChamado(int idChamado)
        {
            return await _context.Chamados
                .AsNoTracking()
                .Include(c => c.Solicitante)
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.ID == idChamado);
        }

        public async Task<bool> GravarChamado(Chamado chamado)
        {
            if (chamado.ID > 0)
            {
                var chamadoExistente = await _context.Chamados.FindAsync(chamado.ID);
                if (chamadoExistente == null)
                {
                    return false;
                }

                chamadoExistente.Assunto = chamado.Assunto;
                chamadoExistente.IdSolicitante = chamado.IdSolicitante;
                chamadoExistente.IdDepartamento = chamado.IdDepartamento;
                chamadoExistente.DataAbertura = chamado.DataAbertura;

                _context.Chamados.Update(chamadoExistente);
            }
            else
            {
                _context.Chamados.Add(chamado);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExcluirChamado(int idChamado)
        {
            var chamado = await _context.Chamados.FindAsync(idChamado);
            if (chamado == null)
            {
                return false;
            }

            _context.Chamados.Remove(chamado);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}