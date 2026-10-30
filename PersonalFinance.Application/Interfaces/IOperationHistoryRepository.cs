using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task<PagedResult<Transaction>> GetHistoryByAccountIdAsync(Guid userId,
            Guid accountId,
            int skip,
            int take,
            CancellationToken cancellationToken,
            OperationType? type = null,
            DateTime? from = null,
            DateTime? to = null);
    }
}
