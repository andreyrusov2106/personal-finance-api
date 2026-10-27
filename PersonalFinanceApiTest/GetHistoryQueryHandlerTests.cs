using Microsoft.Extensions.DependencyInjection;
using PersonalFinance.Application.DTOs;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
using PersonalFinance.Infrastructure;
using PersonalFinanceApiTest.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class GetHistoryQueryHandlerTests
    {
        [TestMethod]
        public async Task After_Deposit_History_Shoud_Have_One_Transaction_Deposit()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountId = createdAccount.Id;


            var depositAccount = await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId);

            // ASSERT (Проверка)
            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(1000, history[0].Amount);
            Assert.AreEqual(OperationType.Deposit, history[0].Type);
            Assert.IsNull(history[0].RelatedAccountName);
            Assert.AreEqual(Direction.Incoming, history[0].Direction);

        }

        [TestMethod]
        public async Task After_Withdrawal_History_Should_Have_One_Outgoing_Transaction()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountId = createdAccount.Id;


            var depositAccount = await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);

            var withdrawalAccount = await userOwnershipTestsHelper.WithdrawAccountAsync(clientA, accountId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId);

            // ASSERT (Проверка)
            Assert.AreEqual(2, history.Count);
            Assert.AreEqual(100, history[0].Amount);
            Assert.AreEqual(OperationType.Withdrawal, history[0].Type);
            Assert.IsNull(history[0].RelatedAccountName);
            Assert.AreEqual(Direction.Outgoing, history[0].Direction);

        }

        [TestMethod]
        public async Task After_Transfer_From_Account_History_Should_Be_Outgoing()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)
            var accountFrom = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountFromId = accountFrom.Id;

            var accountTo = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountToId = accountTo.Id;


            var depositAccount = await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountFromId, 1000);

            await userOwnershipTestsHelper.TransferAsync(clientA, accountFromId, accountToId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountFromId);

            // ASSERT (Проверка)
            Assert.AreEqual(2, history.Count);
            Assert.AreEqual(100, history[0].Amount);
            Assert.AreEqual(OperationType.Transfer, history[0].Type);
            Assert.AreEqual("accountA", history[0].RelatedAccountName);
            Assert.AreEqual(Direction.Outgoing, history[0].Direction);

        }

        [TestMethod]
        public async Task After_Transfer_To_Account_History_Should_Be_Incoming()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)
            var accountFrom = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountFromId = accountFrom.Id;

            var accountTo = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountToId = accountTo.Id;


            var depositAccount = await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountFromId, 1000);

            await userOwnershipTestsHelper.TransferAsync(clientA, accountFromId, accountToId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountToId);

            // ASSERT (Проверка)
            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(100, history[0].Amount);
            Assert.AreEqual(OperationType.Transfer, history[0].Type);
            Assert.AreEqual("accountA", history[0].RelatedAccountName);
            Assert.AreEqual(Direction.Incoming, history[0].Direction);

        }

        [TestMethod]
        public async Task Empty_Account_History_Should_Return_Empty_List()
        {
            // ARRANGE(Подготовка)
            CustomWebApplicationFactory webApplicationFactory = new CustomWebApplicationFactory();
            UserOwnershipTestsHelper userOwnershipTestsHelper = new UserOwnershipTestsHelper();

            var clientA = await userOwnershipTestsHelper.CreateAuthenticatedClientAsync(webApplicationFactory, "userA");

            using var scope = webApplicationFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var currency = new Currency("RUB", 1827);
            db.Currencies.Add(currency);
            await db.SaveChangesAsync();

            // ACT (Действие)
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency);
            var accountId = createdAccount.Id;


            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId);

            // ASSERT (Проверка)
            Assert.AreEqual(0, history.Count);

        }

        [TestMethod]
        public async Task Another_Users_Account_History_Should_Return_NotFound()
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


            var responseGetHistory = await clientB.GetAsync(
              $"api/accounts/{accountId}/history"
            );
            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetHistory.StatusCode);

            

        }
    }
}
