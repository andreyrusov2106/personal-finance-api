using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Exceptions
{
    public class InvalidPaginationParametersException:Exception
    {
        public InvalidPaginationParametersException()
        : base($"Invalid pagination parameters")
        {
        }
    }
}
