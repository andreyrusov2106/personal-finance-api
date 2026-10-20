namespace PersonalFinance.Domain.Exceptions
{
    public class InsufficientFundsException:Exception
    {
        public InsufficientFundsException(decimal amount)
        : this($"Insufficient funds for withdrawal: {amount}")
        {
        }

        public InsufficientFundsException(string detail)
            : base(detail)
        {
        }
    }
}
