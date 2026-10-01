namespace PersonalFinanceApi.Exceptions
{
    public class AccountAlreadyClosedException : Exception
    {
        public AccountAlreadyClosedException(string name)
            : base($"Account with name '{name}' already closed.")
        {
        }
    }
}
