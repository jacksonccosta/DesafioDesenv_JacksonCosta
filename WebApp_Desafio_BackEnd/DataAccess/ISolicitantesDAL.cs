using System.Collections.Generic;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public interface ISolicitantesDAL
    {
        Solicitante ObterSolicitante(int idSolicitante);
        IEnumerable<Solicitante> SearchSolicitantes(string termo);
        IEnumerable<Solicitante> ListarSolicitantes();
    }
}