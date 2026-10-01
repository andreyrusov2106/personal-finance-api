namespace PersonalFinance.Application.Exceptions
{
    public class ConcurrencyException: Exception
    {
        public ConcurrencyException(Exception innerException)
        : base($"The account was modified by another request.", innerException)
        {
        }
    }
}
