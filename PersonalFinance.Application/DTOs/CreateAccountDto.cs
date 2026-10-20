using PersonalFinance.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record CreateAccountDto(
        [property: Required]
        [property: StringLength(100, MinimumLength = 3)]
        string Name,

        [property: NotEmptyGuid]
        Guid CurrencyId
        );
}
