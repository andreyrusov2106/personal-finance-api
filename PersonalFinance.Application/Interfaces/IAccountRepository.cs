using PersonalFinance.Domain;
namespace PersonalFinance.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetAccountAsync(Guid id);
        Task AddAccountAsync(Account account);
        void DeleteAccount(Account account);
    }
}
