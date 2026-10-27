using MediatR;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, IEnumerable<HistoryDto>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOperationHistoryRepository _historyRepository;
        private readonly ICurrentUser _currentUser;

        public GetHistoryQueryHandler(IAccountRepository accountRepository,
            IOperationHistoryRepository historyRepository,
            ICurrentUser currentUser)
        {
            _accountRepository = accountRepository;
            _historyRepository = historyRepository;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<HistoryDto>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountAsync(request.accountId, _currentUser.UserId);
            if (account == null)
                throw new AccountNotFoundException(request.accountId);
            var transactions = await _historyRepository.GetHistoryByAccountId(_currentUser.UserId, request.accountId, cancellationToken);
            List<HistoryDto> historyDtos = new List<HistoryDto>();

            foreach (var transaction in transactions) {
                HistoryDto historyDto = new HistoryDto
                (
                    transaction.Operation.CreatedAt,
                    transaction.Operation.Type,
                    transaction.Operation.Type == OperationType.Transfer ?
                    transaction.Operation.Transactions.First(t => t.AccountId != request.accountId).Account.Name : null,
                    transaction.Operation.Type == OperationType.Deposit ? Direction.Incoming :
                    transaction.Operation.Type == OperationType.Withdrawal ? Direction.Outgoing : transaction.Amount > 0 ? Direction.Incoming : Direction.Outgoing,
                    Math.Abs(transaction.Amount)
                );
                historyDtos.Add( historyDto );

            }
            return historyDtos;
        }
    }
}
