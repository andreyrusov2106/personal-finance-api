namespace PersonalFinanceApi.Exceptions
{
    public class InvalidWithdrawAmountException:Exception
    {
        public InvalidWithdrawAmountException(decimal amount)
            : base($"Invalid withdraw amount: {amount}")
                {
                }
    }
}
