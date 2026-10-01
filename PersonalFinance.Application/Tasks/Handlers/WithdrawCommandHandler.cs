using MediatR;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, AccountItemDto>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WithdrawCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountItemDto> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.accountId);

            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            account.Withdraw(request.Amount);

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
