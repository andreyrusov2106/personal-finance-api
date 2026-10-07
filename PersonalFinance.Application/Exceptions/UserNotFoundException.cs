using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string login)
            : base($"User with login '{login}' was not found.")
        {
        }
    }
}
