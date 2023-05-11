using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Text.Json;
using Tickets.Models;

namespace Tickets.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private const string conflictSqlState = "23505";
        private const string timeoutSqlState = "55P03";
        private const string conflictErrorMsg = "Database conflict error";
        private const string databaseTimeoutErrorMsg = "Database timeout error";
        private const string requestTimeoutErrorMsg = "Request timeout error";
        private const string reqBodyTooLargeExceptionMsg = "Request body too large.";
        private const string reqBodyTooLargeErrorMsg = "Request body too large!";
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
                PostgresException? pge;
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = ApiResponse<string>.Fail(error.Message);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                switch (error)
                {
                    case DbUpdateException ex when ex.InnerException is PostgresException:
                        pge = ex.InnerException as PostgresException;
                        if (pge != null && pge.SqlState == conflictSqlState)
                        {
                            responseModel.Message = conflictErrorMsg;
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                        }
                        break;
                    case InvalidOperationException ex when ex.InnerException is DbUpdateException:
                        pge = ex.InnerException.InnerException as PostgresException;
                        if (pge != null && pge.SqlState == timeoutSqlState)
                        {
                            responseModel.Message = databaseTimeoutErrorMsg;
                            response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        }
                        break;
                    case TaskCanceledException:
                        responseModel.Message = requestTimeoutErrorMsg;
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    case BadHttpRequestException ex:
                        if (ex.Message.Contains(reqBodyTooLargeExceptionMsg))
                        {
                            responseModel.Message = reqBodyTooLargeErrorMsg;
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
