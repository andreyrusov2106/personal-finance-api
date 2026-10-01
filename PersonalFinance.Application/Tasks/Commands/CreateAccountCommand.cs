using MediatR;
using PersonalFinance.Application.DTOs;

namespace PersonalFinance.Application.Tasks.Commands
{
        public record CreateAccountCommand(CreateAccountDto Account) : IRequest<AccountItemDto>;
}
