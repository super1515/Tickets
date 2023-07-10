/*
 * 
 * Middleware необходимый для множественного чтения тела запроса
 * 
 */
namespace Tickets.WebAPI.Middlewares
{
    public class BufferingEnablerMiddleware
    {
        private readonly RequestDelegate _next;

        public BufferingEnablerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            await _next.Invoke(context);
        }
    }
}
