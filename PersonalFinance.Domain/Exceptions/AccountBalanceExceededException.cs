namespace PersonalFinance.Domain.Exceptions
{
    public class AccountBalanceExceededException: Exception
    {
        public AccountBalanceExceededException()
        : base($"Account balance exceeded")
        {
        }

    }
}
