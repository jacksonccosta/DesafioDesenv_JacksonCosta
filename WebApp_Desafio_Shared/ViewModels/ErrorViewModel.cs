using System;
using WebApp_Desafio_Shared.ViewModels.Enums;

namespace WebApp_Desafio_Shared.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId { get; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public AlertTypes Type { get; set; }
    }
}