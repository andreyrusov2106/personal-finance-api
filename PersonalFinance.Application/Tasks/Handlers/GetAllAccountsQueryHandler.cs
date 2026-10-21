using MediatR;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Domain;

namespace PersonalFinance.Application.Tasks.Handlers;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountItemDto>>
{
    private readonly IAccountRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetAllAccountsQueryHandler(IAccountRepository repository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<AccountItemDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAllAsync(_currentUser.UserId);

        // Маппинг коллекции Entity в коллекцию DTO
        return tasks.Select(a => new AccountItemDto(
            a.Id,
            a.Name,
            a.Balance,
            a.CreatedAt,
            a.ClosedAt,
            new CurrencyDto(a.Currency.Id, a.Currency.Code.ToString(), a.Currency.Name)
        ));
    }
}