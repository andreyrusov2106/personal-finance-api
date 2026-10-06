using MediatR;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Domain;
namespace PersonalFinance.Application.Tasks.Handlers
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountItemDto>
    {
        private readonly ICurrencyRepository _curencyRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;


        public CreateAccountCommandHandler(ICurrencyRepository curencyRepository, 
            IAccountRepository accountRepository,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork)
        {
            _curencyRepository = curencyRepository;
            _accountRepository = accountRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountItemDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var currencyId = request.Account.CurrencyId;
            var currency = await _curencyRepository.GetByIdAsync(currencyId);
            if(currency is null)
            {
                throw new CurrencyNotFoundException(currencyId);
            }
            Account newAccount = new Account(request.Account.Name, currency, _currentUser.UserId);
            await _accountRepository.AddAccountAsync(newAccount);
            var result = await _unitOfWork.SaveChangesAsync();
            return new AccountItemDto(newAccount.Id,
                newAccount.Name,
                newAccount.Balance,
                newAccount.CreatedAt,
                newAccount.ClosedAt,
                newAccount.Currency);
        }
    }
}
