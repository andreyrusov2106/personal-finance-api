using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string Login);
        Task AddUserAsync(User user);
    }
}
