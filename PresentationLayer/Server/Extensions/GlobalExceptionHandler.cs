using Microsoft.AspNetCore.Diagnostics;
using Server.Models;

namespace Server.Extensions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new GlobalErrorResponse()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = exception.Message,
                Title = "API is currently injured."
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
