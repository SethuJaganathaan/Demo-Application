namespace Application.API.Extensions
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Application.Repository.DTO.Exception;
    using Application.Service.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError($"An unhandled exception occurred: {exception}");
                await ExceptionHandle(context, exception);
            }
        }

        private Task ExceptionHandle(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorMessage errorMessage = new ErrorMessage
            {
                Status = context.Response.StatusCode,
                Types = context.Response.ContentType,
                Title = exception.Message,
                Detail = exception.StackTrace,
            };

            var result = JsonSerializer.Serialize(errorMessage);
            return context.Response.WriteAsync(result);
        }
    }
}
