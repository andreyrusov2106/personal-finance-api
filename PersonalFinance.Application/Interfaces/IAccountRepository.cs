using PersonalFinance.Domain;
namespace PersonalFinance.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync(Guid userId);
        Task<Account?> GetAccountAsync(Guid id, Guid userId);
        Task AddAccountAsync(Account account);
        void DeleteAccount(Account account);
    }
}
