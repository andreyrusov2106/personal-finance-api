using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Exceptions
{
    public class MismatchedCurrenciesException: Exception
    {
        public MismatchedCurrenciesException()
            : base($"The currencies of the accounts does not match")
        {
        }
    }
}
