using PersonalFinance.Domain;
namespace PersonalFinance.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetByIdAsync(Guid id);
    }
}
