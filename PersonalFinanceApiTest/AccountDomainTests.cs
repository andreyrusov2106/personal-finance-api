using PersonalFinance.Domain;
using PersonalFinance.Domain.Exceptions;

namespace PersonalFinanceApiTest
{
    [TestClass]
    public class AccountDomainTests
    {
        [TestMethod]
        public void Deposit_Should_Not_AllowBalanceAboveMaximum()
        {
            // ARRANGE (Подготовка)
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency, Guid.NewGuid());
            firstAccount.Deposit(99_999_999.99m);
            

            // ACT (Действие)
            // ASSERT (Проверка)
            Assert.ThrowsExactly<AccountBalanceExceededException>(
            (Action)(() =>
            {
                firstAccount.Deposit(0.01m);
            }));

            Assert.AreEqual(99_999_999.99m, firstAccount.Balance);

        }

        [TestMethod]
        public void Deposit_Should_AllowBalanceBelowMaximum()
        {
            // ARRANGE (Подготовка)
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency, Guid.NewGuid());

            // ACT (Действие)
            
             firstAccount.Deposit(1000m);

            // ASSERT (Проверка)

            Assert.AreEqual(1000m, firstAccount.Balance);

        }

        [TestMethod]
        public void Withdraw_Should_Throw_WhenAmountExceedsBalance()
        {
            // ARRANGE (Подготовка)
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency, Guid.NewGuid());


            // ACT (Действие)
            // ASSERT (Проверка)
            Assert.ThrowsExactly<InsufficientFundsException>(
            (Action)(() =>
            {
                firstAccount.Withdraw(100000000m);
            }));

        }

        [TestMethod]
        public void Deposit_Should_Throw_WhenAmountHasMoreThanTwoDecimalPlaces()
        {
            // ARRANGE (Подготовка)
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency, Guid.NewGuid());


            // ACT (Действие)
            // ASSERT (Проверка)
            Assert.ThrowsExactly<InvalidDepositAmountException>(
            (Action)(() =>
            {
                firstAccount.Deposit(10.123m);
            }));

        }

        [TestMethod]
        public void Withdraw_Should_Throw_WhenAmountHasMoreThanTwoDecimalPlaces()
        {
            // ARRANGE (Подготовка)
            var currency = new Currency("RUB", 123);
            var firstAccount = new Account("fist", currency, Guid.NewGuid());
            firstAccount.Deposit(100m);

            // ACT (Действие)
            // ASSERT (Проверка)
            Assert.ThrowsExactly<InvalidWithdrawAmountException>(
            (Action)(() =>
            {
                firstAccount.Withdraw(10.123m);
            }));

        }
    }
}
