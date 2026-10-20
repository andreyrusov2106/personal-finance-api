namespace PersonalFinance.Domain.Exceptions
{
    public class InvalidDepositAmountException : Exception
    {
        public InvalidDepositAmountException(decimal amount)
            : base($"Invalid deposit amount: {amount}")
        {
        }
    }
}
