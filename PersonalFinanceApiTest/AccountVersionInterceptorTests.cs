using Microsoft.EntityFrameworkCore;
using PersonalFinanceApi;
using PersonalFinance.Infrastructure.Concurency;
using PersonalFinance.Domain;
using PersonalFinanceApi.Exceptions;
using PersonalFinance.Application.Tasks.Handlers;
using PersonalFinance.Application.Tasks.Commands;
using PersonalFinance.Application.Tasks.Queries;
using PersonalFinance.Application.Exceptions;
using PersonalFinance.Infrastructure;
using PersonalFinance.Infrastructure.Repositories;

namespace PersonalFinanceApiTest
{

    [TestClass]
    public class AccountVersionInterceptorTests
    {
        /*
        [TestMethod]
        public void Version_Should_Increase_When_Account_Is_Modified()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var currency = new Currency("RUB", 123);
            var account = new Account("test", currency);
            context.Accounts.Add(account);

            // ACT (Действие)
            context.SaveChanges();

            account.Deposit(100);
            context.SaveChanges();
            
            account.Withdraw(100);
            context.SaveChanges();


            // ASSERT (Проверка)
            Assert.AreEqual(2, account.Version);
        }

        [TestMethod]
        public void Second_Context_Should_Throw_ConcurrencyException_When_Account_Was_Changed()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var firstContext = new AppDbContext(options);
            var currency = new Currency("RUB", 123);
            var account = new Account("test", currency);
            firstContext.Accounts.Add(account);

            var secondContext = new AppDbContext(options);

            // ACT (Действие)
            firstContext.SaveChanges();

            account.Deposit(1000);
            firstContext.SaveChanges();


            var accountA = firstContext.Accounts.Single(x => x.Id == account.Id);
            var accountB = secondContext.Accounts.Single(x => x.Id == account.Id);


            accountA.Withdraw(100);
            firstContext.SaveChanges();

            accountB.Withdraw(100);

            // ASSERT (Проверка)
            Assert.ThrowsExactly<ConcurrencyException>(() =>
            {
                secondContext.SaveChanges();
            });
        }

        [TestMethod]
        public async Task Success_Transfer_100_From_One_Account_To_Another()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;
            
            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency);
            context.Accounts.Add(firstAccount);
            var secondAccount = new Account("second", currency);
            context.Accounts.Add(secondAccount);


            // ACT (Действие)
            firstAccount.Deposit(1000);
            secondAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);

           await handler.Handle(
                new TransferCommand(firstAccount.Id, secondAccount.Id, 100),
                CancellationToken.None);

            var firstAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id);
            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id);



            // ASSERT (Проверка)
            Assert.AreEqual(900, firstAccountAfterTransfer.Balance);
            Assert.AreEqual(1100, secondAccountAfterTransfer.Balance);

        }

        [TestMethod]
        public async Task Not_Enough_Money_For_Transfer_Balace_Should_Be_The_Same()
        { // ARRANGE (Подготовка)
          var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options; var context = new AppDbContext(options); 
            var repository = new AccountRepository(context); 
            var currency = new Currency("RUB", 123); 
            var firstAccount = new Account("fist", currency); 
            context.Accounts.Add(firstAccount); 
            var secondAccount = new Account("second", currency); 
            context.Accounts.Add(secondAccount); 
            firstAccount.Deposit(1000); 
            secondAccount.Deposit(1000); 
            context.SaveChanges(); 
            var handler = new TransferCommandHandler(repository, context); 
            await Assert.ThrowsExactlyAsync<InsufficientFundsException>(
                async () => { await handler.Handle( 
                    new TransferCommand(firstAccount.Id, secondAccount.Id, 1500), CancellationToken.None); }); 
            var firstAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id); 
            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id); 
            // ASSERT (Проверка)
            Assert.AreEqual(1000, firstAccountAfterTransfer.Balance); 
            Assert.AreEqual(1000, secondAccountAfterTransfer.Balance); 
        }

            [TestMethod]
        public async Task Unsuccess_Transfer_For_Two_The_Same_Ids()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency);
            context.Accounts.Add(firstAccount);



            firstAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);

            await Assert.ThrowsExactlyAsync<SameAccountTransferException>(async () =>
            {
                await handler.Handle(
                new TransferCommand(firstAccount.Id, firstAccount.Id, 1500),
                CancellationToken.None);
            });

            var accountAfterTransfer =
            context.Accounts.Single(x => x.Id == firstAccount.Id);

            Assert.AreEqual(1000, accountAfterTransfer.Balance);

        }

        [TestMethod]
        public async Task Unsuccess_Transfer_With_Amount_Less_Than_Zero()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency);
            context.Accounts.Add(firstAccount);
            var secondAccount = new Account("second", currency);
            context.Accounts.Add(secondAccount);


            // ACT (Действие)
            firstAccount.Deposit(1000);
            secondAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);


            await Assert.ThrowsExactlyAsync<InvalidTransferAmountException>(async () =>
            {
                await handler.Handle(
                new TransferCommand(firstAccount.Id, secondAccount.Id, -1500),
                CancellationToken.None);
            });

            var firstAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id);
            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id);



            // ASSERT (Проверка)
            Assert.AreEqual(1000, firstAccountAfterTransfer.Balance);
            Assert.AreEqual(1000, secondAccountAfterTransfer.Balance);

        }

        [TestMethod]
        public async Task Unsuccess_Transfer_With_Amount_Equals_Zero()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency);
            context.Accounts.Add(firstAccount);
            var secondAccount = new Account("second", currency);
            context.Accounts.Add(secondAccount);


            // ACT (Действие)
            firstAccount.Deposit(1000);
            secondAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);


            await Assert.ThrowsExactlyAsync<InvalidTransferAmountException>(async () =>
            {
                await handler.Handle(
                new TransferCommand(firstAccount.Id, secondAccount.Id, 0),
                CancellationToken.None);
            });

            var firstAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id);
            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id);



            // ASSERT (Проверка)
            Assert.AreEqual(1000, firstAccountAfterTransfer.Balance);
            Assert.AreEqual(1000, secondAccountAfterTransfer.Balance);

        }

        [TestMethod]
        public async Task Unsuccess_Transfer_With_Not_Found_First_Account()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var secondAccount = new Account("second", currency);
            context.Accounts.Add(secondAccount);


            // ACT (Действие)
            secondAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);


            await Assert.ThrowsExactlyAsync<AccountNotFoundException>(async () =>
            {
                await handler.Handle(
                new TransferCommand(new Guid(), secondAccount.Id, 1000),
                CancellationToken.None);
            });

            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == secondAccount.Id);



            // ASSERT (Проверка)
            Assert.AreEqual(1000, secondAccountAfterTransfer.Balance);

        }

        [TestMethod]
        public async Task Unsuccess_Transfer_With_Not_Found_Second_Account()
        {
            // ARRANGE (Подготовка)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .AddInterceptors(new AccountVersionInterceptor())
                .Options;

            var context = new AppDbContext(options);
            var repository = new AccountRepository(context);
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("first", currency);
            context.Accounts.Add(firstAccount);


            // ACT (Действие)
            firstAccount.Deposit(1000);
            context.SaveChanges();

            var handler = new TransferCommandHandler(repository, context);


            await Assert.ThrowsExactlyAsync<AccountNotFoundException>(async () =>
            {
                await handler.Handle(
                new TransferCommand(firstAccount.Id, new Guid(), 1000),
                CancellationToken.None);
            });

            var secondAccountAfterTransfer = context.Accounts.Single(x => x.Id == firstAccount.Id);



            // ASSERT (Проверка)
            Assert.AreEqual(1000, secondAccountAfterTransfer.Balance);

        }*/



    }
}
