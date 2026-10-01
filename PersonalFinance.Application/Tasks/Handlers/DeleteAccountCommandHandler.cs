using MediatR;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Exceptions;


namespace PersonalFinance.Application.Tasks.Handlers
{
    public class DeleteAccountCommandHandler: IRequestHandler<DeleteAccountCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.accountId);

            if (account == null)
                throw new AccountNotFoundException(request.accountId);

            _repository.DeleteAccount(account);

            await _unitOfWork.SaveChangesAsync();

        }
    }
}
