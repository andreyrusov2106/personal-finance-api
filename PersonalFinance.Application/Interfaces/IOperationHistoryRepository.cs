using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task<PagedResult<Transaction>> GetHistoryByAccountIdAsync(Guid userId, Guid accountId, int skip, int take, CancellationToken cancellationToken);
    }
}
