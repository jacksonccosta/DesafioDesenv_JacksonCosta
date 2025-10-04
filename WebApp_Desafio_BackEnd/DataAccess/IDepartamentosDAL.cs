using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface IDepartamentosDAL
    {
        IEnumerable<Departamento> ListarDepartamentos();
        Departamento ObterDepartamento(int id);
        bool GravarDepartamento(Departamento departamento);
        bool ExcluirDepartamento(int id);
    }
}