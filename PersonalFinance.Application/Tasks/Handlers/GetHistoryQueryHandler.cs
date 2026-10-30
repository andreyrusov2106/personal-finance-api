using MediatR;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class GetHistoryQueryHandler : IRequestHandler<GetHistoryQuery, PaginationHistoryDto>
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

        public async Task<PaginationHistoryDto> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
        {
            if(request.page<1 ||
                (request.pageSize<1 || request.pageSize >100))
            {
                throw new InvalidParametersException();
            }
            if (request.from.HasValue && request.to.HasValue && request.from.Value.Date > request.to.Value.Date)
            {
                throw new InvalidParametersException();
            }
            var account = await _accountRepository.GetAccountAsync(request.accountId, _currentUser.UserId);
            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            
            int skip=(request.page - 1)*request.pageSize;
            var transactions = await _historyRepository.GetHistoryByAccountIdAsync(_currentUser.UserId,
                request.accountId, 
                skip, 
                request.pageSize, 
                cancellationToken,
                request.type,
                request.from,
                request.to);
            List<HistoryDto> historyDtos = new List<HistoryDto>();

            foreach (var transaction in transactions.Items) {
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

            return new PaginationHistoryDto(
                historyDtos, 
                request.page, 
                request.pageSize, 
                transactions.TotalCount,
                (int)Math.Ceiling((decimal)transactions.TotalCount / (decimal)request.pageSize)
                );
        }
    }
}
