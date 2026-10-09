using Microsoft.AspNetCore.Diagnostics;
using PersonalFinanceApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceApi
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionMapper _exceptionMapper;

        public GlobalExceptionHandler(IExceptionMapper exceptionMapper)
        {
            _exceptionMapper = exceptionMapper;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var apiException = _exceptionMapper.Map(exception);

            if (apiException is not null)
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
