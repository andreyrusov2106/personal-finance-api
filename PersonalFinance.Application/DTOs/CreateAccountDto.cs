using PersonalFinance.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record CreateAccountDto(
        [Required]
        [StringLength(100, MinimumLength = 3)]
        string Name,

        [NotEmptyGuid]
        Guid CurrencyId
        );
}
