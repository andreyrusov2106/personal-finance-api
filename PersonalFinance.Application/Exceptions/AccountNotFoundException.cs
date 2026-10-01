
namespace PersonalFinance.Application.Exceptions
{
    public class AccountNotFoundException:Exception
    {
        public AccountNotFoundException(Guid accountId)
            : base($"Account with id '{accountId}' was not found.")
        {
        }
    }
}
