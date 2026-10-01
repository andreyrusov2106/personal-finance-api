using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record DepositCommand(Guid accountId, decimal Amount) : IRequest<AccountItemDto>
    {
    }
}
