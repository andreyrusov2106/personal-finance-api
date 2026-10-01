using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApi;

namespace PersonalFinance.Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Accounts
               .AsNoTracking()
               .Include(a => a.Currency)
               .ToListAsync();
        }

        public async Task<Account?> GetAccountAsync(Guid id)
        {
            return await _context.Accounts
                //.AsNoTracking()
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAccountAsync(Account account)
        {
            _context.Entry(account.Currency).State = EntityState.Unchanged;
            Console.WriteLine($"Repository context: {_context.GetHashCode()}");
            await _context.Accounts.AddAsync(account);
            var entry = _context.Entry(account);


            Console.WriteLine($"Account state: {entry.State}");
        }

        public void DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
        }
    }
}
