using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace UserAuthManagement.Services.ErrorController
{
    public class GlobalErrorHandling : IExceptionHandler
    {
        private readonly ILogger<GlobalErrorHandling> _logger;

        public GlobalErrorHandling(ILogger<GlobalErrorHandling> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occured!");

            var statusCode = (int) HttpStatusCode.InternalServerError;
            var message = "An unhandled exception occured";


            if(exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Not Found";
            }

            if(exception is InvalidOperationException)
            {
                statusCode = (int) HttpStatusCode.BadRequest;
                message = "Invalid Operation";
            }

            if(exception is BadHttpRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            if(exception is UnauthorizedAccessException)
            {
                statusCode = (int) HttpStatusCode.Unauthorized;
                message = "Unauthorized User";
            }

            if(exception is SqlException)
            {
                statusCode = (int) HttpStatusCode.InternalServerError;
                message = "Server Issue. Try again";
            }

            if(exception is ArgumentNullException)
            {
                statusCode = (int) HttpStatusCode.BadRequest;
                message = "Null object can't be send";
            }

            if(exception is ArgumentException)
            {
                statusCode = (int) HttpStatusCode.BadRequest;
                message = "Invalid";
            }


            var res = new ErrorMessage {
                StatusCode = statusCode,
                Message = message,
                Details = exception.Message
            };


            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            var result = JsonSerializer.Serialize(res);
            await httpContext.Response.WriteAsync(result);

            return true; //Error handled successfully

        }

    }
}
