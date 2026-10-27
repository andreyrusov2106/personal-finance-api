using System.ComponentModel.DataAnnotations;


namespace PersonalFinance.Application.DTOs
{
    public record RegisterRequestDto(
    [Required]
    [StringLength(100, MinimumLength = 3)]
    string Login,

    [Required]
    [MinLength(8)]
    [MaxLength(128)]
    string Password,

    [Required]
    [StringLength(100, MinimumLength = 1)]
    string Name);
}
