namespace PersonalFinance.Application.Exceptions
{
    public class SameAccountTransferException : Exception
    {
        public SameAccountTransferException()
            : base($"Accounts are the same")
        {
        }
    }
}
