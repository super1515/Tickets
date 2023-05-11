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
                switch (error)
                {
                    case DbUpdateException ex when ex.InnerException is PostgresException:
                        pge = ex.InnerException as PostgresException;
                        if (pge != null && pge.SqlState == conflictSqlState)
                        {
                            responseModel.Message = "Database conflict error";
                            response.StatusCode = (int)HttpStatusCode.Conflict;
                        }
                        break;
                    case InvalidOperationException ex when ex.InnerException is DbUpdateException:
                        pge = ex.InnerException.InnerException as PostgresException;
                        if (pge != null && pge.SqlState == timeoutSqlState)
                        {
                            responseModel.Message = "Database timeout error";
                            response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        }
                        break;
                    case TaskCanceledException:
                        responseModel.Message = "Request timeout error";
                        response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                        break;
                    default:
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
