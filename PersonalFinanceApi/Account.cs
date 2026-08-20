namespace PersonalFinanceApi
{
    public class Account
    {
        public Account(string name, decimal balace, Currency currency)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balace = balace;
            Currency = currency;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Balace { get; private set; }
        public Currency Currency { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ClosedAt { get; private set; }
    }
}
