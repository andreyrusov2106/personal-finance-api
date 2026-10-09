using PersonalFinance.Application.Exceptions;

namespace PersonalFinanceApi.Exceptions
{
    public class ApiExceptionMapper : IExceptionMapper
    {
        public IApiException? Map(Exception exception)
        {
            return exception switch
            {
                AccountNotFoundException ex => new ApiException(
                    StatusCodes.Status404NotFound,
                    "Account not found",
                    ex.Message),

                _ => null
            };
        }
    }
}