using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record LoginCommand(LoginRequestDto LoginRequestDto) : IRequest<string>
    {
    }
}
