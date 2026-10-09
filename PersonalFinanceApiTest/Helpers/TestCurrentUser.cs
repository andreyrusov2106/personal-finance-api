using PersonalFinance.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinanceApiTest.Helpers
{
    public class TestCurrentUser(Guid userId) : ICurrentUser
    {
        public Guid UserId => userId;
    }
}
