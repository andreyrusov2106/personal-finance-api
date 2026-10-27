using MediatR;
using PersonalFinance.Application.DTOs;


namespace PersonalFinance.Application.Tasks.Queries
{
    public record GetHistoryQuery(Guid accountId, int page, int pageSize) : IRequest<PaginationHistoryDto>;
}
