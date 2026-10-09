namespace PersonalFinanceApi.Exceptions
{
    public class ApiException : IApiException
    {
        public int Status { get; }
        public string Title { get; }
        public string Detail { get; }

        public ApiException(int status, string title, string detail)
        {
            Status = status;
            Title = title;
            Detail = detail;
        }
    }
}
