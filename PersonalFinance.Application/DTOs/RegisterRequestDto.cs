using System.ComponentModel.DataAnnotations;


namespace PersonalFinance.Application.DTOs
{
    public record RegisterRequestDto(
    [property: Required]
    [property: StringLength(100, MinimumLength = 3)]
    string Login,

    [property: Required]
    [property: MinLength(8)]
    [property: MaxLength(128)]
    string Password,

    [property: Required]
    [property: StringLength(100, MinimumLength = 1)]
    string Name);
}
