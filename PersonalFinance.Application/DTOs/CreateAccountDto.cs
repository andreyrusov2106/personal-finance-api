namespace PersonalFinance.Application.DTOs
{
    public record CreateAccountDto(
        string Name,
        Guid CurrencyId
        );
}
