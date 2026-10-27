using MediatR;
using PersonalFinance.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Tasks.Queries
{
    public record GetHistoryQuery(Guid accountId) : IRequest<IEnumerable<HistoryDto>>;
}
