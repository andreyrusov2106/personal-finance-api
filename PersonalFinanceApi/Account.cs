namespace PersonalFinanceApi
{
    public class Account
    {
        public Account(string name, Currency currency)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balace = 0;
            Currency = currency;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public decimal Balace { get; private set; }
        public Currency Currency { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }

        public void Deposit(decimal sum, Currency currency)
        {
            if (!currency.Equals(Currency))
            {
                sum = ConvertToCurrency(sum, currency, Currency);
            }
            Balace += sum;
        }

        private decimal ConvertToCurrency(decimal sum, Currency currency1, Currency currency2)
        {
            throw new NotImplementedException();
        }

        public void Withdraw(decimal sum, Currency currency)
        {
            if (!currency.Equals(Currency))
            {
                sum = ConvertToCurrency(sum, currency, Currency);
            }
            Balace -= sum;
        }
        public void Close()
        {
            ClosedAt = DateTime.UtcNow;
        }
    }

    
}
