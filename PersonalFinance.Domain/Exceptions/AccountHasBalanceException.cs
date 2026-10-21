using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Domain.Exceptions
{
    public class AccountHasBalanceException:Exception
    {
        public AccountHasBalanceException(decimal balance)
            : base($"Account has balance'{balance}'")
        {
        }
    }
}
