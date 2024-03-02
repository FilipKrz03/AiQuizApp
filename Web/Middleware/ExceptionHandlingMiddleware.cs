
using Domain.Exceptions;

namespace Web.Middleware
{
    public sealed class ExceptionHandlingMiddleware(
        ILogger<ExceptionHandlingMiddleware> logger
        ) : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ResourceNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(InvalidAccesTokenException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("ExceptionHandlingMiddleware - {ex}", ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Error occured on server side");
            }
        }
    }
}
