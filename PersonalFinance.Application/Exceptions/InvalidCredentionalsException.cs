namespace PersonalFinance.Application.Exceptions
{
    public class InvalidCredentionalsException : Exception
    {
        public InvalidCredentionalsException()
            : base($"Invalid credentionals")
        {
        }

    }
}

