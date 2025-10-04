using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface IChamadosDAL
    {
        IEnumerable<Chamado> ListarChamados();
        Chamado ObterChamado(int idChamado);
        bool GravarChamado(Chamado chamado);
        bool ExcluirChamado(int idChamado);
    }
}