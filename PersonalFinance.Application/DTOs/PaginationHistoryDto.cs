using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record class PaginationHistoryDto(
        List<HistoryDto> items,
        int page,
        int pageSize,
        int totalCount,
        int totalPages);

}
