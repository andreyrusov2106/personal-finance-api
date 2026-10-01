using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using PersonalFinanceApi;

namespace PersonalFinance.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly AppDbContext _context;

        public CurrencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Currency?> GetByIdAsync(Guid id)
        {
            return await _context
                .Currencies
                .AsNoTracking ()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
