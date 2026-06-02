using Application.Exceptions;
using System.Net;
using System.Text.Json;
namespace Presentation.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = exception switch
            {
                AppException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            context.Response.StatusCode = statusCode;
            var response = new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Detailed = statusCode == 500 ? "An unexpected error occurred." : null
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}