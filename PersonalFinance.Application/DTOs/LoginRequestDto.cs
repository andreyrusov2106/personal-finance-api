using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record LoginRequestDto(
        [Required]
        [StringLength(100, MinimumLength = 3)]
        string login,

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        string password
        );

}
