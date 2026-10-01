namespace PersonalFinance.Application.Exceptions
{
    public class CurrencyNotFoundException : Exception
    {
        public CurrencyNotFoundException(Guid currencyId)
        : base($"Currency with id '{currencyId}' was not found.")
        {
        }
    }
}
