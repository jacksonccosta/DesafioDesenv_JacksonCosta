using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface IChamadosDAL
    {
        Task<IEnumerable<Chamado>> ListarChamados();
        Task<Chamado> ObterChamado(int idChamado);
        Task<bool> GravarChamado(Chamado chamado);
        Task<bool> ExcluirChamado(int idChamado);
    }
}