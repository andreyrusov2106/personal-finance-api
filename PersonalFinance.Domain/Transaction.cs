using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Domain
{
    public class Transaction
    {
        public Transaction()
        {
        }

        public Transaction(Guid operationId, Guid accountId, decimal amount)
        {
            Id = Guid.NewGuid();
            OperationId = operationId;
            AccountId = accountId;
            Amount = amount;
        }

        public Guid Id { get; private set; }
        public Guid OperationId { get; private set; }
        public Operation Operation { get; private set; }
        public Guid AccountId { get; private set; }
        public Account Account { get; private set; }
        public decimal Amount { get; private set; }
    }
}
