namespace PersonalFinanceApi.Exceptions
{
    public class InsufficientFundsException:Exception
    {
        public InsufficientFundsException(decimal amount)
            : this($"Invalid withdraw amount: {amount}")
                {
                }

        public InsufficientFundsException(string detail)
        {
        }
    }
}
