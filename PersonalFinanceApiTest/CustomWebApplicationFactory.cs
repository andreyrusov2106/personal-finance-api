using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Concurency;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PersonalFinanceApiTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            builder.ConfigureServices(services =>
            {
                if (services.FirstOrDefault(x => x.ServiceType == typeof(AppDbContext)) is { } service)
                {
                    services.Remove(service);
                }

                services.AddDbContext<AppDbContext>((serviceProvider, options) =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("TestConnection"));
                    options.AddInterceptors(
                        serviceProvider.GetRequiredService<AccountVersionInterceptor>());
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureDeleted();
                db.Database.Migrate();
            });
        }
    }
}
