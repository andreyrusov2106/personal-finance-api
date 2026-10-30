using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;

namespace PersonalFinance.Infrastructure.Repositories
{
    public class OperationHistoryRepository : IOperationHistoryRepository
    {
        private readonly AppDbContext _context;

        public OperationHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Transaction>> GetHistoryByAccountIdAsync(
            Guid userId, 
            Guid accountId, 
            int skip, 
            int take, 
            CancellationToken cancellationToken,
            OperationType? type=null,
            DateTime? from =null,
            DateTime? to = null)
        {
            IQueryable<Transaction> baseQuery = _context.Transactions
            .Where(t => t.AccountId == accountId && t.Account.UserId == userId);

            if (type != null) baseQuery = baseQuery.Where(t=>t.Operation.Type==type.Value);
            if (from != null) baseQuery = baseQuery.Where(t => t.Operation.CreatedAt>=from.Value);
            if (to != null) baseQuery = baseQuery.Where(t => t.Operation.CreatedAt < to.Value.AddDays(1));

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var items = await baseQuery
                .Include(t => t.Operation)
                    .ThenInclude(o => o.Transactions)
                        .ThenInclude(t => t.Account)
                .OrderByDescending(t => t.Operation.CreatedAt)
                .ThenBy(t => t.Id)
                .AsNoTrackingWithIdentityResolution()
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            return new PagedResult<Transaction>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
