using System.Collections.Generic;
using WebServerProject.Models;

namespace WebServerProject.Services
{
    class AccountService : IAccountService
    {
        private static List<Account> accounts = new List<Account>();

        static AccountService()
        {
            accounts.Add(new Account { Id = 1, Login = "admin", Password = "admin" });
            accounts.Add(new Account { Id = 2, Login = "user", Password = "user" });
        }

        public static string currentToken { get; set; }
        public static string currentRole { get; set; }
        public static int? currentAccountID { get; set; }

        public void Add(Account account)
        {
            accounts.Add(account);
        }

        public List<Account> GetAccounts()
        {
            return accounts;
        }
    }
}
