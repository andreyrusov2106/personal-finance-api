using MediatR;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record DeleteAccountCommand(Guid accountId) : IRequest
    {
    }
}
