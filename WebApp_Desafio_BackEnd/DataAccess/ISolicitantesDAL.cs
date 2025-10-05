using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface ISolicitantesDAL
    {
        Task<Solicitante> ObterSolicitante(int idSolicitante);
        Task<IEnumerable<Solicitante>> SearchSolicitantes(string termo);
        Task<IEnumerable<Solicitante>> ListarSolicitantes();
    }
}