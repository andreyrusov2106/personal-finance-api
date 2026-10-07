using PersonalFinance.Application.Interfaces;

namespace PersonalFinanceApi
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.NameIdentifier);

                if(claim is null)
                {
                    throw new UnauthorizedAccessException();
                }
                
                if (!Guid.TryParse(claim.Value, out Guid result))
                {
                    throw new InvalidOperationException();
                }
                return result;

            }            
        }
    }
}
