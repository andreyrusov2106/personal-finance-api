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

        public DepositCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountItemDto> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.accountId);

            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            account.Deposit(request.Amount);

            await _unitOfWork.SaveChangesAsync();

            return new AccountItemDto(
                account.Id,
                account.Name,
                account.Balance,
                account.CreatedAt,
                account.ClosedAt,
                account.Currency
            );
        }
    }
}
