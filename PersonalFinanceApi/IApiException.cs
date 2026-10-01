namespace PersonalFinanceApi
{
    public interface IApiException
    {
        int Status { get; }
        string Title { get; }
        string Detail { get; }
    }
}
