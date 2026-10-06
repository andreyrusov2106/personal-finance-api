using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinance.Domain
{
    public class User
    {
        private User()
        {
        }

        public User(string login, string name)
        {
            Id = Guid.NewGuid();
            Login = login;
            Name = name;
            Role = "User";
        }

        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;
        public string Name { get; private set; }
        public string Role { get; private set; }

        public ICollection<Account> Accounts { get; private set; }
            = new List<Account>();

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
