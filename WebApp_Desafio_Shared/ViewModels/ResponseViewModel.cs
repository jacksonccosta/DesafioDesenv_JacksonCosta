using System;
using WebApp_Desafio_Shared.ViewModels.Enums;

namespace WebApp_Desafio_Shared.ViewModels
{
    public class ResponseViewModel : ErrorViewModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public AlertTypes Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public ResponseViewModel(string message)
        {
            Message = message;
        }

        public ResponseViewModel(
            string title,
            string message,
            AlertTypes type,
            string controller = "",
            string action = "")
        {
            Title = title;
            Message = message;
            Type = type;
            Controller = controller;
            Action = action;
        }

        public ResponseViewModel(Exception exception)
        {
            Type = AlertTypes.error;
            Title = "Ocorreu um erro!";
            Action = "Error";
            Message = exception.Message;
        }
    }
}