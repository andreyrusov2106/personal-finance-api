using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record TransferAccountDto(
    Guid ToAccountId,
    decimal Amount
);
}
