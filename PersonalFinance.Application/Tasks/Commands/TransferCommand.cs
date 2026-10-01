using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record TransferCommand(Guid FromAccountId, Guid ToAccountId, decimal Amount) : IRequest
    {
    }
}
