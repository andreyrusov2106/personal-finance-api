using MediatR;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Exceptions;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, AccountItemDto>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUser _currentUser;

        public CloseAccountCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<AccountItemDto> Handle(
            CloseAccountCommand request,
            CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.accountId, _currentUser.UserId);

            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            account.Close();

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
