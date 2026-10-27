using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Infrastructure.Repositories
{
    public class OperationHistoryRepository : IOperationHistoryRepository
    {
        private readonly AppDbContext _context;

        public OperationHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetHistoryByAccountId(Guid userId, Guid accountId, CancellationToken cancellationToken)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId && t.Account.UserId== userId)
                .Include(t => t.Operation).ThenInclude(o=> o.Transactions).ThenInclude(t => t.Account)
                .OrderByDescending(t => t.Operation.CreatedAt)
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync(cancellationToken);
        }
    }
}
