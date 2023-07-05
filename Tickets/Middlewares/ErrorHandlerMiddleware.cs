using System.Net;
using System.Text.Json;
using Tickets.Infrastructure.Models;
using Tickets.Infrastructure.Exceptions;
/*
 * 
 * Middleware необходимый для обработки исключений
 * 
 */
namespace Tickets.WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private const string requestTimeoutErrorMsg = "Request timeout error.";
        private const string reqBodyTooLargeExceptionMsg = "Request body too large.";
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = ApiResponse<string>.Fail(error.Message);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                switch (error)
                {
                    case ConflictException ex:
                        responseModel.Message = ex.Message;
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case RequestTimeoutException ex:
                        responseModel.Message = ex.Message;
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    case TaskCanceledException:
                        responseModel.Message = requestTimeoutErrorMsg;
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    case BadHttpRequestException ex:
                        if (ex.Message.Contains(reqBodyTooLargeExceptionMsg))
                        {
                            responseModel.Message = reqBodyTooLargeExceptionMsg;
                            response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                        }
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
