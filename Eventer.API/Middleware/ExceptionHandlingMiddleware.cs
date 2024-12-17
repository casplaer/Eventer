using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Eventer.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An error occurred");

            int statusCode = StatusCodes.Status500InternalServerError; 
            string message = "An unexpected error occurred.";

            switch (exception)
            {
                case ArgumentException or ArgumentNullException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;

                case ValidationException validationException:
                    statusCode = StatusCodes.Status400BadRequest;

                    var firstErrorMessage = validationException.Errors?.FirstOrDefault()?.ErrorMessage
                        ?? "Ошибка валидации.";
                    message = firstErrorMessage;

                    break;

                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = exception.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = exception.Message;
                    break;

                case InvalidOperationException:
                    statusCode = StatusCodes.Status409Conflict;
                    message = exception.Message;
                    break;

                default:
                    _logger.LogWarning("Unhandled exception type: {ExceptionType}", exception.GetType());
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Message = message
            });
        }
    }

}