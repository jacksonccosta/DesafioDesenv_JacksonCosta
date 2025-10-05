using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : IDepartamentosDAL
    {
        private readonly ApplicationDbContext _context;

        public DepartamentosDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departamento>> ListarDepartamentos()
        {
            return await _context.Departamentos
                                 .OrderBy(d => d.Descricao)
                                 .ToListAsync();
        }

        public async Task<Departamento> ObterDepartamento(int id)
        {
            return await _context.Departamentos.FindAsync(id);
        }

        public async Task<bool> GravarDepartamento(Departamento departamento)
        {
            if (departamento.ID > 0)
            {
                _context.Departamentos.Update(departamento);
            }
            else
            {
                _context.Departamentos.Add(departamento);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExcluirDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return false;
            }

            _context.Departamentos.Remove(departamento);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}