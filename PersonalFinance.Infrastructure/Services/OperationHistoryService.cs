using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Infrastructure.Services
{
    public class OperationHistoryService : IOperationHistoryService
    {
        private readonly AppDbContext _appDbContext;

        public OperationHistoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void RecordDeposit(Guid accountId, decimal amount)
        {
            Operation operation = new Operation(OperationType.Deposit);
            Transaction transaction = new Transaction(operation.Id, accountId, amount);
            operation.AddTransaction(transaction);
            _appDbContext.Operations.Add(operation);
        }

        public void RecordTransfer(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            Operation operation = new Operation(OperationType.Transfer);
            Transaction transactionWithdrawal = new Transaction(operation.Id, fromAccountId, -amount);
            Transaction transactionDeposit = new Transaction(operation.Id, toAccountId, amount);
            
            operation.AddTransaction(transactionWithdrawal);
            operation.AddTransaction(transactionDeposit);
            _appDbContext.Operations.Add(operation);
        }

        public void RecordWithdrawal(Guid accountId, decimal amount)
        {
            Operation operation = new Operation(OperationType.Withdrawal);
            Transaction transaction = new Transaction(operation.Id, accountId, -amount);
            operation.AddTransaction(transaction);
            _appDbContext.Operations.Add(operation);
        }
    }
}
