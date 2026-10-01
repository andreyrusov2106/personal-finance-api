using MediatR;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Interfaces;

namespace PersonalFinance.Application.Tasks.Handlers;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountItemDto>>
{
    private readonly IAccountRepository _repository;

    public GetAllAccountsQueryHandler(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AccountItemDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAllAsync();

        // Маппинг коллекции Entity в коллекцию DTO
        return tasks.Select(a => new AccountItemDto(
            a.Id,
            a.Name,
            a.Balance,
            a.CreatedAt,
            a.ClosedAt,
            a.Currency
        ));
    }
}