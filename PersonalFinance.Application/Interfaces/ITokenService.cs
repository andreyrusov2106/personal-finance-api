namespace PersonalFinance.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string userId, string role, string name);
    }
}
