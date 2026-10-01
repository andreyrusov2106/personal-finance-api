using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PersonalFinance.Domain;

namespace PersonalFinance.Infrastructure.Concurency
{
    public class AccountVersionInterceptor: SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateVersions(eventData);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            UpdateVersions(eventData);
            return new(result);
        }

        private void UpdateVersions(DbContextEventData eventData)
        {
            var modifiedAccounts = eventData.Context.ChangeTracker
                .Entries<Account>()
                .Where(a => a.State == EntityState.Modified);

            foreach (var account in modifiedAccounts)
            {
                account.Property(x => x.Version).CurrentValue++;
            }
        }
    }
}
