namespace PersonalFinance.Domain
{
    public class Currency
    {
        public Currency(string name, int code)
        {
            Id = Guid.NewGuid();
            Name = name;
            Code = code;
        }

        public Currency(Guid id, string name, int code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Code { get; private set; }

        public override bool Equals(object? obj)
        {
            return obj is Currency currency &&
                   Code == currency.Code;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code);
        }


    }
}