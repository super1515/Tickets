using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using System.Text.Json;
using Tickets.WebAPI.Models;
using Tickets.WebAPI.Services.Interfaces;
/*
 * 
 * Фильтр для валидации тела запроса JSON схемой
 * 
 */
namespace Tickets.WebAPI.Filters
{
    public class ValidateWithJsonSchemeFilter : Attribute, IAsyncResourceFilter
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string? _message;
        private const string responseContentType = "application/json";
        public ValidateWithJsonSchemeFilter(HttpStatusCode statusCode, string message)
        {
            _statusCode = statusCode;
            _message = message;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetRequiredService<ISchemasValidatorService>();
            var version = context.HttpContext.GetRequestedApiVersion()!;
            var endpoint = context.HttpContext.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.OfType<ControllerActionDescriptor>().First()!;
            string body = await ReadBodyAsync(context.HttpContext.Request);
            if (actionDescriptor == null || !validator.ContentIsValidBySchema(actionDescriptor, version, body))
            {
                await WriteResponseAsync(context.HttpContext.Response);
                return;
            }
            await next();
        }
        private async Task WriteResponseAsync(HttpResponse response)
        {
            response.StatusCode = (int)_statusCode;
            if (_message == null) return;
            response.ContentType = responseContentType;
            var responseModel = ApiResponse<string>.Fail(_message);
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
        private async Task<string> ReadBodyAsync(HttpRequest request)
        {
            string body;
            request.Body.Position = 0;
            using (StreamReader stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                body = await stream.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }
}
