using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record WithdrawAccountDto(
        [Range(typeof(decimal), "0,01", "1000000000")]
        decimal Amount);
}
