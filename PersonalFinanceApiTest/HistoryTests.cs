using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Enums;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Concurency;
using PersonalFinance.Infrastructure.Repositories;
using PersonalFinance.Infrastructure.Services;
using PersonalFinanceApiTest.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public async Task Success_Transfer_100_From_One_Account_To_Another_History_Shoud_Have_One_Operation_And_Two_Transaction()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var service = new OperationHistoryService(context);
            var currency = new Currency("RUB", 123);
            var user = new User("test", "Test User");

            context.Users.Add(user);


            var firstAccount = new Account("fist", currency, user.Id);
            context.Accounts.Add(firstAccount);
            var secondAccount = new Account("second", currency, user.Id);
            context.Accounts.Add(secondAccount);


            // ACT (Действие)
            firstAccount.Deposit(1000);
            secondAccount.Deposit(1000);
            context.SaveChanges();

            var currentUser = new TestCurrentUser(user.Id);

            var handler = new TransferCommandHandler(repository, context, currentUser, service);

            await handler.Handle(
                 new TransferCommand(firstAccount.Id, secondAccount.Id, 100),
                 CancellationToken.None);

            var firstAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id);
            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id);

            var transactions = context.Transactions.ToList();
            var operations = context.Operations.ToList();


            // ASSERT (Проверка)
            Assert.AreEqual(900, firstAccountAfterTransfer.Balance);
            Assert.AreEqual(1100, secondAccountAfterTransfer.Balance);
            Assert.AreEqual(2, transactions.Count);
            Assert.AreEqual(100, transactions[0].Amount);
            Assert.AreEqual(-100, transactions[1].Amount);
            Assert.AreEqual(secondAccount.Id, transactions[0].AccountId);
            Assert.AreEqual(firstAccount.Id, transactions[1].AccountId);
            Assert.AreEqual(operations[0].Id, transactions[0].OperationId);
            Assert.AreEqual(operations[0].Id, transactions[1].OperationId);
            Assert.AreEqual(1, operations.Count);
            Assert.AreEqual(OperationType.Transfer, operations[0].Type);

        }

        [TestMethod]
        public async Task Success_Deposit_100_History_Shoud_Have_One_Operation_And_One_Transaction()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var service = new OperationHistoryService(context);
            var currency = new Currency("RUB", 123);
            var user = new User("test", "Test User");

            context.Users.Add(user);


            var account = new Account("fist", currency, user.Id);
            context.Accounts.Add(account);
            context.SaveChanges();


            // ACT (Действие)
            var currentUser = new TestCurrentUser(user.Id);

            var handler = new DepositCommandHandler(repository, context, currentUser, service);

            await handler.Handle(
                 new DepositCommand(account.Id, 100),
                 CancellationToken.None);

            var accountAfterDeposit = context.Accounts.Single(x => x.Id == account.Id);

            var transactions = context.Transactions.ToList();
            var operations = context.Operations.ToList();


            // ASSERT (Проверка)
            Assert.AreEqual(100, accountAfterDeposit.Balance);
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(100, transactions[0].Amount);
            Assert.AreEqual(account.Id, transactions[0].AccountId);
            Assert.AreEqual(1, operations.Count);
            Assert.AreEqual(OperationType.Deposit, operations[0].Type);

        }

        [TestMethod]
        public async Task Success_Withdrawal_100_History_Shoud_Have_One_Operation_And_One_Transaction()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var service = new OperationHistoryService(context);
            var currency = new Currency("RUB", 123);
            var user = new User("test", "Test User");

            context.Users.Add(user);


            var account = new Account("fist", currency, user.Id);
            context.Accounts.Add(account);
            account.Deposit(1000);
            context.SaveChanges();


            // ACT (Действие)
            var currentUser = new TestCurrentUser(user.Id);

            var handler = new WithdrawCommandHandler(repository, context, currentUser, service);

            await handler.Handle(
                 new WithdrawCommand(account.Id, 100),
                 CancellationToken.None);

            var accountAfterWithdraw = context.Accounts.Single(x => x.Id == account.Id);

            var transactions = context.Transactions.ToList();
            var operations = context.Operations.ToList();


            // ASSERT (Проверка)
            Assert.AreEqual(900, accountAfterWithdraw.Balance);
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(-100, transactions[0].Amount);
            Assert.AreEqual(account.Id, transactions[0].AccountId);
            Assert.AreEqual(1, operations.Count);
            Assert.AreEqual(OperationType.Withdrawal, operations[0].Type);

        }
    }
}
