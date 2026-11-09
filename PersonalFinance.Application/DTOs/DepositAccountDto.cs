using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Application.DTOs
{
    public record DepositAccountDto(
    [property: Range(
        typeof(decimal), 
        "0.01", 
        "99999999.99",
        ParseLimitsInInvariantCulture = true,
        ConvertValueInInvariantCulture = true)]
    decimal Amount);
}
