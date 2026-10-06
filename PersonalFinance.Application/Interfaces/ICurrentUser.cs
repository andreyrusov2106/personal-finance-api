using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
    }
}
