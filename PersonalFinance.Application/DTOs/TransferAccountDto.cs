using PersonalFinance.Application.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalFinance.Application.DTOs
{
    public record TransferAccountDto(
        [property: NotEmptyGuid]
        Guid ToAccountId,

        [property: Range(typeof(decimal), "0.01", "1000000000")]
        decimal Amount
);
}
