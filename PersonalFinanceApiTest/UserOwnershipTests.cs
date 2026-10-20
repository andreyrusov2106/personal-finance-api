using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Domain;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Repositories;
using PersonalFinanceApiTest.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class UserOwnershipTests
    {
        [TestMethod]
        public async Task User_Cant_Get_Other_User_Account()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");
            var clientB = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userB");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)

            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountId = createdAccount.Id;
            var responseGetAccount = await clientB.GetAsync(
              $"api/accounts/{accountId}"
            );

            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetAccount.StatusCode);

        }

        [TestMethod]
        public async Task User_Cant_Close_Other_User_Account()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");
            var clientB = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userB");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)

            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountId = createdAccount.Id;
            var responseGetAccount = await clientB.PutAsync(
              $"api/accounts/{accountId}", null
            );

            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetAccount.StatusCode);

        }

        [TestMethod]
        public async Task User_Cant_Withdraw_From_Other_User_Account()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");
            var clientB = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userB");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)

            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);

            var accountId = createdAccount.Id;
            var responseGetAccount = await clientB.PostAsJsonAsync(
              $"/api/accounts/{accountId}/withdraw", new WithdrawAccountDto(1000)
            );

            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetAccount.StatusCode);

        }

        [TestMethod]
        public async Task User_Cant_Deposit_To_Other_User_Account()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");
            var clientB = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userB");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)

            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);

            var accountId = createdAccount.Id;
            var responseGetAccount = await clientB.PostAsJsonAsync(
              $"/api/accounts/{accountId}/deposit", new DepositAccountDto(1000)
            );

            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetAccount.StatusCode);

        }

        [TestMethod]
        public async Task User_Cant_Transfer_From_Other_User_Account()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");
            var clientB = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userB");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)

            var createdAccountA = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var createdAccountB = await userOwnershipTestsHelper.CreateAccountAsync(clientB, currency);

            var accountIdA = createdAccountA.Id;
            var accountIdB = createdAccountB.Id;
            var responseTransfer = await clientB.PostAsJsonAsync(
              $"/api/accounts/{accountIdA}/transfer", new TransferAccountDto(accountIdB, 1000)
            );

            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseTransfer.StatusCode);

        }

        [TestMethod]
        public async Task User_Cant_Transfer_More_Than_Balance()
        {
            // ARRANGE
            var factory = new CustomWebApplicationFactory();
            var helper = new UserOwnershipTestsHelper();

            var client = await helper.CreateAuthenticatedClientAsync(factory, "userA");

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            var sourceAccount = await helper.CreateAccountAsync(client, currency);
            var destinationAccount = await helper.CreateAccountAsync(client, currency);

            // ACT
            var response = await client.PostAsJsonAsync(
                $"/api/accounts/{sourceAccount.Id}/transfer",
                new TransferAccountDto(destinationAccount.Id, 1000));

            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            // ASSERT
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(problem);
            Assert.AreEqual("Insufficient funds", problem.Title);
        }



    }
}
