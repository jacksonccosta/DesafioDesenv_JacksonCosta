namespace WebApp_Desafio_BackEnd.Shared
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Result Ok() => new Result { Success = true };

        public static Result Fail(string message) => new Result { Success = false, Message = message };
    }
}