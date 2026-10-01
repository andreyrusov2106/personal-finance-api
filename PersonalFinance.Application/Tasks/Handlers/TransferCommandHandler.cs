using MediatR;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            if (request.FromAccountId == request.ToAccountId)
                throw new SameAccountTransferException();

            if (request.Amount <= 0)
                throw new InvalidTransferAmountException(request.Amount);

            var fromAccount = await _repository.GetAccountAsync(request.FromAccountId);

            var toAccount = await _repository.GetAccountAsync(request.ToAccountId);

            if (fromAccount == null)
                throw new AccountNotFoundException(request.FromAccountId);
            if (toAccount == null)
                throw new AccountNotFoundException(request.ToAccountId);

            fromAccount.Withdraw(request.Amount);
            toAccount.Deposit(request.Amount);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
