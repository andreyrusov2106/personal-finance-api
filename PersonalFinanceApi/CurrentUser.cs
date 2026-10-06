using PersonalFinance.Application.Interfaces;

namespace PersonalFinanceApi
{
    public class CurrentUser : ICurrentUser
    {
        public Guid UserId => throw new NotImplementedException();
    }
}
