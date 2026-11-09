using PersonalFinance.Application.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record TransferAccountDto(
        [NotEmptyGuid]
        Guid ToAccountId,

        [property: Range(
        typeof(decimal),
        "0.01",
        "99999999.99",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true)]
        decimal Amount
);
}
