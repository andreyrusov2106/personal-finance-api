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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId,1,20);

            // ASSERT (Проверка)
            Assert.AreEqual(1, history.items.Count);
            Assert.AreEqual(1000, history.items[0].Amount);
            Assert.AreEqual(OperationType.Deposit, history.items[0].Type);
            Assert.IsNull(history.items[0].RelatedAccountName);
            Assert.AreEqual(Direction.Incoming, history.items[0].Direction);

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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);

            await userOwnershipTestsHelper.WithdrawAccountAsync(clientA, accountId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 1, 20);

            // ASSERT (Проверка)
            Assert.AreEqual(2, history.items.Count);
            Assert.AreEqual(100, history.items[0].Amount);
            Assert.AreEqual(OperationType.Withdrawal, history.items[0].Type);
            Assert.IsNull(history.items[0].RelatedAccountName);
            Assert.AreEqual(Direction.Outgoing, history.items[0].Direction);

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
            var accountFrom = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountFromId = accountFrom.Id;

            var accountTo = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountB");
            var accountToId = accountTo.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountFromId, 1000);

            await userOwnershipTestsHelper.TransferAsync(clientA, accountFromId, accountToId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountFromId, 1, 20);

            // ASSERT (Проверка)
            Assert.AreEqual(2, history.items.Count);
            Assert.AreEqual(100, history.items[0].Amount);
            Assert.AreEqual(OperationType.Transfer, history.items[0].Type);
            Assert.AreEqual("accountB", history.items[0].RelatedAccountName);
            Assert.AreEqual(Direction.Outgoing, history.items[0].Direction);

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
            var accountFrom = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountFromId = accountFrom.Id;

            var accountTo = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountB");
            var accountToId = accountTo.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountFromId, 1000);

            await userOwnershipTestsHelper.TransferAsync(clientA, accountFromId, accountToId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountToId, 1, 20);

            // ASSERT (Проверка)
            Assert.AreEqual(1, history.items.Count);
            Assert.AreEqual(100, history.items[0].Amount);
            Assert.AreEqual(OperationType.Transfer, history.items[0].Type);
            Assert.AreEqual("accountA", history.items[0].RelatedAccountName);
            Assert.AreEqual(Direction.Incoming, history.items[0].Direction);

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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 1, 20);

            // ASSERT (Проверка)
            Assert.AreEqual(0, history.items.Count);
            Assert.AreEqual(0, history.totalCount);
            Assert.AreEqual(0, history.totalPages);

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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            var responseGetHistory = await clientB.GetAsync(
              $"api/accounts/{accountId}/history"
            );
            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.NotFound, responseGetHistory.StatusCode);

            

        }

        [TestMethod]
        public async Task After_5Deposit_History_With_Pagination2_Shoud_Have_3Pages()
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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 1, 2);

            // ASSERT (Проверка)
            Assert.AreEqual(2, history.items.Count);
            Assert.AreEqual(5, history.totalCount);
            Assert.AreEqual(3, history.totalPages);


        }

        [TestMethod]
        public async Task Get_History_With_Pagination101_Should_Return_BadRequest()
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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);


            var responseGetHistory = await clientA.GetAsync(
              $"api/accounts/{accountId}/history?page=1&pageSize=101"
            );
            // ASSERT (Проверка)
            Assert.AreEqual(HttpStatusCode.BadRequest, responseGetHistory.StatusCode);



        }

        [TestMethod]
        public async Task After_5Deposit_History_With_Pagination2_Shoud_Have_5Different_Transactions()
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
            var createdAccount = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountId = createdAccount.Id;


            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 1000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 2000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 3000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 4000);
            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountId, 5000);

            var historyPage1 = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 1, 2);
            var historyPage2 = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 2, 2);
            var historyPage3 = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountId, 3, 2);

            var allSum = historyPage1.items.Select(i => i.Amount)
                .Concat(historyPage2.items.Select(i => i.Amount))
                .Concat(historyPage3.items.Select(i => i.Amount));
            allSum = allSum.Distinct();
            // ASSERT (Проверка)
            Assert.AreNotEqual(historyPage1.items[0].Amount, historyPage1.items[1].Amount);
            Assert.DoesNotContain(historyPage1.items[0].Amount, historyPage2.items);
            Assert.DoesNotContain(historyPage1.items[0].Amount, historyPage3.items);
            Assert.AreEqual(2, historyPage1.items.Count);
            Assert.AreEqual(2, historyPage2.items.Count);
            Assert.AreEqual(1, historyPage3.items.Count);
            Assert.AreEqual(5, allSum.ToList().Count);


        }

        [TestMethod]
        public async Task History_With_Deposit_Filter_Should_Return_Only_Deposits()
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
            var accountFrom = await userOwnershipTestsHelper.CreateAccountAsync(clientA, currency, "accountA");
            var accountFromId = accountFrom.Id;

            await userOwnershipTestsHelper.DepositAccountAsync(clientA, accountFromId, 1000);

            await userOwnershipTestsHelper.WithdrawAccountAsync(clientA, accountFromId, 100);

            var history = await userOwnershipTestsHelper.GetHistoryAsync(clientA, accountFromId, 1, 20,OperationType.Deposit);

            // ASSERT (Проверка)
            Assert.AreEqual(1, history.items.Count);
            Assert.AreEqual(1000, history.items[0].Amount);
            Assert.AreEqual(OperationType.Deposit, history.items[0].Type);
            Assert.IsNull(history.items[0].RelatedAccountName);
            Assert.AreEqual(Direction.Incoming, history.items[0].Direction);
            Assert.AreEqual(1, history.totalCount);

        }

    }
}
