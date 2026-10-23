using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface IOperationHistoryService
    {
        void RecordDeposit(Guid accountId, decimal amount);
        void RecordWithdrawal(Guid accountId, decimal amount);
        void RecordTransfer(Guid fromAccountId, Guid toAccountId, decimal amount);
    }
}
