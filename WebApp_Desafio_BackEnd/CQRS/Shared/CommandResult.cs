namespace WebApp_Desafio_BackEnd.CQRS.Shared
{
    public class CommandResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        private CommandResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static CommandResult Ok() => new CommandResult(true, "Operação realizada com sucesso.");

        public static CommandResult Fail(string message) => new CommandResult(false, message);
    }
}