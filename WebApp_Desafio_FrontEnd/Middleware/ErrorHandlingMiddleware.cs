using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApp_Desafio_Shared.ViewModels;

namespace WebApp_Desafio_FrontEnd.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Console.WriteLine(exception);

            var code = HttpStatusCode.InternalServerError;
            var message = "Ocorreu um erro inesperado no servidor.";

            if (exception is ApplicationException)
            {
                code = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            var result = JsonConvert.SerializeObject(new ResponseViewModel(message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            var code = HttpStatusCode.BadRequest;

            var message = string.Join(" ", exception.Errors.Select(e => e.ErrorMessage));

            var result = JsonConvert.SerializeObject(new ResponseViewModel(message));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}