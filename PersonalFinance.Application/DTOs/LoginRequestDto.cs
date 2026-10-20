using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record LoginRequestDto(
        [property: Required]
        [property: StringLength(100, MinimumLength = 3)]
        string login,

        [property: Required]
        [property: MinLength(8)]
        [property: MaxLength(128)]
        string password
        );

}
