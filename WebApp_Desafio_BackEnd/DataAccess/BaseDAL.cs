namespace WebApp_Desafio_BackEnd.DataAccess
{
    public abstract class BaseDAL
    {
        protected readonly string CONNECTION_STRING;

        protected BaseDAL(string connectionString)
        {
            CONNECTION_STRING = connectionString;
        }
    }
}