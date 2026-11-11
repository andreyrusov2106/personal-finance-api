using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
using PersonalFinance.Infrastructure;
using PersonalFinanceApiTest.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class TransferRollbackTests
    {
        [TestMethod]
        public async Task Transfer_Should_Rollback_When_Transaction_Save_Fails()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            context.Currencies.Add(currency);
            await context.SaveChangesAsync();

            // ACT (Действие)

            var createdAccountA = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var createdAccountB = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");

            await userOwnershipTestsHelper.DepositAccountAsync(clientA, createdAccountA.Id, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, createdAccountB.Id, 1000);

            try
            {
                await context.Database.ExecuteSqlRawAsync("""
                ALTER TABLE "Transactions"
                ADD CONSTRAINT "CK_Test_Transfer_Rollback"
                CHECK ("Amount" <> -1000);
                """);

                var accountIdA = createdAccountA.Id;
                var accountIdB = createdAccountB.Id;
            
                var responseTransfer = await clientA.PostAsJsonAsync(
                  $"/api/accounts/{accountIdA}/transfer", new TransferAccountDto(accountIdB, 1000)
                );
                Assert.AreEqual(HttpStatusCode.InternalServerError, responseTransfer.StatusCode);



                using var newScope = webApplicationFactory.Services.CreateScope();

                var newContext = newScope.ServiceProvider.GetRequiredService<AppDbContext>();

                var accountAAfterTransfer = await newContext.Accounts.SingleAsync(a => a.Id == accountIdA);
                var accountBAfterTransfer = await newContext.Accounts.SingleAsync(a => a.Id == accountIdB);

                var operations = await newContext.Operations.Where(o => o.Type == OperationType.Transfer).ToListAsync();
                var transactions = await newContext.Transactions.Where(t=>t.Operation.Type== OperationType.Transfer).ToListAsync();

                Assert.AreEqual(1000, accountAAfterTransfer.Balance);
                Assert.AreEqual(1000, accountBAfterTransfer.Balance);
                Assert.AreEqual(0, operations.Count);
                Assert.AreEqual(0, transactions.Count);
            }

            
            finally{
                await context.Database.ExecuteSqlRawAsync("""
                ALTER TABLE "Transactions"
                DROP CONSTRAINT IF EXISTS "CK_Test_Transfer_Rollback";
                """);

            }
        }
    }
}
