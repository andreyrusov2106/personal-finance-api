using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Exceptions
{
    public class InvalidParametersException:Exception
    {
        public InvalidParametersException()
        : base($"Invalid pagination parameters")
        {
        }
    }
}
