using MediatR;
using PersonalFinance.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Tasks.Commands
{
    public record RegisterCommand(RegisterRequestDto RegisterRequestDto) : IRequest<Guid>;
}
