using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Domain;
using PersonalFinance.Application.Interfaces;

namespace PersonalFinance.Infrastructure
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException(ex);
            }
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException(ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Balance).HasColumnType("decimal(10,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Version).IsConcurrencyToken();

                entity.HasOne<Currency>(a=>a.Currency)
                  .WithMany()
                  .HasForeignKey(a => a.CurrencyId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(30); ;
                entity.HasIndex(s => s.Code).IsUnique();
            });

            modelBuilder.Entity<Currency>().HasData(
            new
            {
                Id = Guid.Parse("b0d4ce5d-2757-4699-948c-cfa72ba94001")
            ,
                Name = "RUB"
            ,
                Code = 643
            },
            new
            {
                Id = Guid.Parse("b0d4ce5d-2757-4699-948c-cfa72ba94002")
            ,
                Name = "USD"
            ,
                Code = 840
            },
            new
            {
                Id = Guid.Parse("b0d4ce5d-2757-4699-948c-cfa72ba94003")
            ,
                Name = "EUR"
            ,
                Code = 978
            },
            new
            {
                Id = Guid.Parse("b0d4ce5d-2757-4699-948c-cfa72ba94004")
            ,
                Name = "CNY"
            ,
                Code = 156
            }
            );

        }
    }
}
