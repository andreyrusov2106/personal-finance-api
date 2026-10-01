using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Queries;

// Запрос тоже IRequest, но возвращает коллекцию.
public record GetAllAccountsQuery : IRequest<IEnumerable<AccountItemDto>>;


