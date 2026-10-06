using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Exceptions
{
    public class LoginIsNotUniqueException : Exception
    {
        public LoginIsNotUniqueException(string Login)
            : base($"User with login '{Login}' already added.")
        {
        }
    }
}
