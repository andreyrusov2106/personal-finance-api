namespace PersonalFinance.Application.Exceptions
{
    public class InvalidTransferAmountException : Exception
    {
        public InvalidTransferAmountException(decimal amount)
            : base($"Invalid transfer amount: {amount}")
        {
        }
    }
}
