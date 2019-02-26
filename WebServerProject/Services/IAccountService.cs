using System.Collections.Generic;
using WebServerProject.Models;

namespace WebServerProject.Services
{
    public interface IAccountService
    {
        List<Account> GetAccounts();
        void Add(Account account);
    }
}
