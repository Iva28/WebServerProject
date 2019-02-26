using WebServerProject.Server.Attributes;
using WebServerProject.Services;

namespace WebServerProject.Controllers
{
    public class AccountController
    {
        private IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpMethod("GET")]
        public string Login()
        {
            AccountService.currentAccountID = null;
            AccountService.currentToken = null;
            return $"<form method='POST' action='http://127.0.0.1:5600/account/enter'>" +
                  $"<div style='box-sizing: border-box; width:280px; height: 50px; position: relative;'><label style='margin:3px; position: absolute; left:5px; top: 3px;'><b>Login</b></label>" +
                  $"<input style='display: inline-block; border: 1px solid #ccc; padding: 5px; margin: 3px; position: absolute; right:5px;' type = 'text' name='login' required></div>" +
                  $"<div style='box-sizing: border-box; width:280px; height: 50px; position: relative;'><label style='margin:3px; position: absolute; left:5px; top: 3px;'><b>Password </b></label>" +
                  $"<input style='display: inline-block; border: 1px solid #ccc; padding: 5px; margin: 3px; position: absolute; right:5px;' type = 'password' name='password' required></div>" +
                  $"<input type = 'submit' value='Enter' style='height:25px; width: 267px; margin-left: 5px;'></form>";
        }

        [HttpMethod("POST")]
        public string Enter(string login, string password)
        {
            var found = accountService.GetAccounts().Find(a => a.Login == login && a.Password == password);
            if (found != null)
            {
                AccountService.currentAccountID = found.Id;
                return $"<script>window.location = 'http://127.0.0.1:5600/account/home'</script>";
            }
            else
            {
                return $"<script>window.location = 'http://127.0.0.1:5600/account/login'</script>";
            }
        }

        [HttpMethod("GET")]
        public string Home()
        {
            return $"<h3 style='margin-left:10px;margin-top:15px;margin-bottom:0;'>Home</h3><div style='margin:10px;'>" +
                $"<a href='http://127.0.0.1:5600/toDo/all' style='font-size:15px;color:black;'>My ToDoList</a></div>" +
                $"<div style='margin:10px;'><a href='http://127.0.0.1:5600/account/login' style='font-size:15px;color:black;'>Exit</a></div>";
        }
    }
}
