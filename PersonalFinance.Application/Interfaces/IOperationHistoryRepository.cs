using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task<IEnumerable<Transaction>> GetHistoryByAccountId(Guid userId, Guid accountId, CancellationToken cancellationToken);
    }
}
