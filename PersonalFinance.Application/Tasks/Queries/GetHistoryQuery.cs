using MediatR;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain.Enums;


namespace PersonalFinance.Application.Tasks.Queries
{
    public record GetHistoryQuery(Guid accountId,
        int page, 
        int pageSize,
        OperationType? type = null,
        DateTime? from = null,
        DateTime? to = null) : IRequest<PaginationHistoryDto>;
}
