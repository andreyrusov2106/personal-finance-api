using Microsoft.EntityFrameworkCore;
using PersonalFinance.Application.Interfaces;
using PersonalFinance.Domain;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace PersonalFinance.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserAsync(string login)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Login== login);
        }
    }
}
