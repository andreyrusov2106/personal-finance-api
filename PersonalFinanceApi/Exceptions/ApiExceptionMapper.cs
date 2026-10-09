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

                ConcurrencyException ex => new ApiException(
                StatusCodes.Status409Conflict,
                "The account was modified by another request",
                ex.Message),

                CurrencyNotFoundException ex => new ApiException(
                    StatusCodes.Status404NotFound,
                    "Currency not found",
                    ex.Message),

                InvalidCredentionalsException ex => new ApiException(
                StatusCodes.Status400BadRequest,
                "Invalid credentionals",
                ex.Message),

                InvalidTransferAmountException ex => new ApiException(
                StatusCodes.Status400BadRequest,
                "Invalid Transfer Amount",
                ex.Message),

                LoginIsNotUniqueException ex => new ApiException(
                StatusCodes.Status409Conflict,
                "Login Is Not Unique",
                ex.Message),

                SameAccountTransferException ex => new ApiException(
                StatusCodes.Status400BadRequest,
                "Accounts are the same",
                ex.Message),

                UserNotFoundException ex => new ApiException(
                    StatusCodes.Status404NotFound,
                    "User not found",
                    ex.Message),

                _ => null
            };
        }
    }
}