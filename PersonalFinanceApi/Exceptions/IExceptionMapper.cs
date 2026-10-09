namespace PersonalFinanceApi.Exceptions
{
    public interface IExceptionMapper
    {
        IApiException? Map(Exception exception);
    }
}
