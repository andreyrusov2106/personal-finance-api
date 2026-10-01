using Microsoft.AspNetCore.Diagnostics;
using PersonalFinanceApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceApi
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is IApiException apiException)
            {
                httpContext.Response.StatusCode = apiException.Status;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = apiException.Title,
                    Status = apiException.Status,
                    Detail = apiException.Detail
                }, cancellationToken);

                return true;
            }

            return false;
        }
    }
}
