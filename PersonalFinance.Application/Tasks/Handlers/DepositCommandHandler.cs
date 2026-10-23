using MediatR;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Exceptions;


namespace PersonalFinance.Application.Tasks.Handlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, AccountItemDto>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IOperationHistoryService _operationHistoryService;

        public DepositCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IOperationHistoryService operationHistoryService)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _operationHistoryService = operationHistoryService;
        }

        public async Task<AccountItemDto> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.accountId, _currentUser.UserId);

            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            account.Deposit(request.Amount);

            _operationHistoryService.RecordDeposit(account.Id, request.Amount);

            await _unitOfWork.SaveChangesAsync();

            return new AccountItemDto(
                account.Id,
                account.Name,
                account.Balance,
                account.CreatedAt,
                account.ClosedAt,
                new CurrencyDto(account.Currency.Id, account.Currency.Code.ToString(), account.Currency.Name)
            );
        }
    }
}
