using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record WithdrawCommand(Guid accountId, decimal Amount) : IRequest<AccountItemDto>
    {
    }
}
