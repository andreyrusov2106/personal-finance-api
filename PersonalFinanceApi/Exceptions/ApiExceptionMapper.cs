using PersonalFinance.Application.Exceptions;
using PersonalFinance.Domain.Exceptions;
using System.Security.Authentication;

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

                InvalidCredentialsException ex => new ApiException(
                StatusCodes.Status400BadRequest,
                "Invalid Credentials",
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

                AccountAlreadyClosedException ex => new ApiException(
                StatusCodes.Status409Conflict,
                "Account already closed",
                ex.Message),

                AccountHasBalanceException ex => new ApiException(
                    StatusCodes.Status400BadRequest,
                    "Account has balance",
                    ex.Message),

                InsufficientFundsException ex => new ApiException(
                    StatusCodes.Status400BadRequest,
                    "Insufficient funds",
                    ex.Message),

                InvalidDepositAmountException ex => new ApiException(
                    StatusCodes.Status400BadRequest,
                    "Invalid deposit amount",
                    ex.Message),

                InvalidWithdrawAmountException ex => new ApiException(
                    StatusCodes.Status400BadRequest,
                    "Invalid withdraw amount",
                    ex.Message),
                InvalidPaginationParametersException ex => new ApiException(
                   StatusCodes.Status400BadRequest,
                   "\"Invalid pagination parameters",
                   ex.Message),

                MismatchedCurrenciesException ex => new ApiException(
                StatusCodes.Status400BadRequest,
                "The currencies of the accounts does not match",
                ex.Message),

                _ => null
            };
        }
    }
}