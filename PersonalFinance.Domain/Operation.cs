using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

namespace PersonalFinance.Domain
{
    public class Operation
    {
        public Operation(OperationType type)
        {
            if (!Enum.IsDefined(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }
            Id = Guid.NewGuid();
            Type = type;
            CreatedAt = DateTime.UtcNow;
        }
        public Operation()
        {
        }

        public Guid Id { get; private set; }
        public OperationType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<Transaction> Transactions => _transactions;

        private readonly List<Transaction> _transactions = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            if (transaction==null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }
            if (transaction.OperationId != Id)
            {
                throw new InvalidOperationException("Operation id don't match");
            }
            _transactions.Add(transaction);
        }
    }
}
