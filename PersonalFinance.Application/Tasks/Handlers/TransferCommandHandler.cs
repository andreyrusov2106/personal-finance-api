using MediatR;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Domain;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class TransferCommandHandler : IRequestHandler<TransferCommand>
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUser _currentUser;
        private readonly IOperationHistoryService _operationHistoryService;

        public TransferCommandHandler(IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IOperationHistoryService operationHistoryService)
        {
            _repository = accountRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _operationHistoryService = operationHistoryService;
        }
        public async Task Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            if (request.FromAccountId == request.ToAccountId)
                throw new SameAccountTransferException();

            if (request.Amount <= 0)
                throw new InvalidTransferAmountException(request.Amount);

            var fromAccount = await _repository.GetAccountAsync(request.FromAccountId, _currentUser.UserId);

            var toAccount = await _repository.GetAccountAsync(request.ToAccountId, _currentUser.UserId);

            

            if (fromAccount == null)
                throw new AccountNotFoundException(request.FromAccountId);
            if (toAccount == null)
                throw new AccountNotFoundException(request.ToAccountId);

            if (!fromAccount.Currency.Equals(toAccount.Currency))
            {
                throw new MismatchedCurrenciesException();
            }

            fromAccount.Withdraw(request.Amount);
            toAccount.Deposit(request.Amount);

            _operationHistoryService.RecordTransfer(fromAccount.Id, toAccount.Id, request.Amount);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
