using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record CloseAccountCommand(Guid accountId) : IRequest<AccountItemDto>
    {
    }
}
