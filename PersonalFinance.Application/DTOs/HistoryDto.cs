using PersonalFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record HistoryDto(
        DateTime CreatedAt,
        OperationType Type,
        string? RelatedAccountName,
        Direction Direction,
        decimal Amount
        );
}
