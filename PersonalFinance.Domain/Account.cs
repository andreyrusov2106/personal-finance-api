using PersonalFinanceApi.Exceptions;

namespace PersonalFinance.Domain
{
    public class Account
    {
        public Account(string name, Currency currency, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty.", nameof(userId));

            Id = Guid.NewGuid();
            Name = name;
            Balance = 0;
            Currency = currency;
            CurrencyId = currency.Id;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
        }
        private Account()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }
        public int Version { get; private set; }
        public Guid CurrencyId { get; private set; }
        public Currency Currency { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public void Deposit(decimal Amount)
        {
            if (Amount <= 0)
            {
                throw new InvalidDepositAmountException(Amount);
            }
            if(ClosedAt is not null)
            {
                throw new AccountAlreadyClosedException(Name);
            }
            Balance += Amount;
        }

        private decimal ConvertToCurrency(decimal sum, Currency currency1, Currency currency2)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(decimal Amount)
        {
            if (Amount <= 0)
            {
                throw new InvalidWithdrawAmountException(Amount);
            }
            if (ClosedAt is not null)
            {
                throw new AccountAlreadyClosedException(Name);
            }
            if (Amount > Balance)
            {
                throw new InsufficientFundsException(Amount);
            }


            Balance -= Amount;
        }
        public void Close()
        {
            if (ClosedAt != null)
                throw new AccountAlreadyClosedException(Name);

            ClosedAt = DateTime.UtcNow;
        }

        public void ChangeName(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                Name=name;
            }
            else { throw new InvalidOperationException("Переданно неккоректное имя аккаунта"); }
        }
    }

    
}
