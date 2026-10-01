using MediatR;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Exceptions;

namespace PersonalFinance.Application.Tasks.Handlers
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, AccountItemDto?>
    {
        private readonly IAccountRepository _repository;

        public GetAccountQueryHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<AccountItemDto?> Handle(
            GetAccountQuery request,
            CancellationToken cancellationToken)
        {
            var account = await _repository.GetAccountAsync(request.Id);

            if (account == null)
                throw new AccountNotFoundException(request.Id);

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
