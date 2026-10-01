using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Queries
{
    public record GetAccountQuery(Guid Id) : IRequest<AccountItemDto?>;
}
