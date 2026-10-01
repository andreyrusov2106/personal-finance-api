using PersonalFinance.Domain;
namespace PersonalFinance.Application.DTOs
{

    public record AccountItemDto(
    Guid Id,
    string Name,
    decimal Balance,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    Currency Currency
);

}
