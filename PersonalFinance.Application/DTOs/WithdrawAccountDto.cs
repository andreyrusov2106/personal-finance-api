using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record WithdrawAccountDto(
        [Range(typeof(decimal), "0.01", "99999999.99")]
        decimal Amount);
}
