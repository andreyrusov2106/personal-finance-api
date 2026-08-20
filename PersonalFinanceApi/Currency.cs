namespace PersonalFinanceApi
{
    public class Currency
    {
        public Currency(string name, int code)
        {
            Id = Guid.NewGuid();
            Name = name;
            Code = code;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Code { get; private set; }

    }
}