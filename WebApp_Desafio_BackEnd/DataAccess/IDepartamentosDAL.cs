using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface IDepartamentosDAL
    {
        Task<IEnumerable<Departamento>> ListarDepartamentos();
        Task<Departamento> ObterDepartamento(int id);
        Task<bool> GravarDepartamento(Departamento departamento);
        Task<bool> ExcluirDepartamento(int id);
    }
}