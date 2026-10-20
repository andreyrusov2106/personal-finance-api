using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record CurrencyDto(
    Guid Id,
    string Code,
    string Name);
}
