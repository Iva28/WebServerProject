using System;
using System.Net;
using WebServerProject.Server;
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

        public HttpListenerContext Context { get; set; }

        [HttpMethod("GET")]
        public string Login()
        {
            AccountService.currentAccountID = null;
            AccountService.currentToken = null;
            AccountService.currentRole = null;
            return $"<form method='POST' action='http://127.0.0.1:5600/account/authenticate'>" +
                  $"<div style='box-sizing: border-box; width:280px; height: 50px; position: relative;'><label style='margin:3px; position: absolute; left:5px; top: 3px;'><b>Login</b></label>" +
                  $"<input style='display: inline-block; border: 1px solid #ccc; padding: 5px; margin: 3px; position: absolute; right:5px;' type = 'text' name='login' required value='user'></div>" +
                  $"<div style='box-sizing: border-box; width:280px; height: 50px; position: relative;'><label style='margin:3px; position: absolute; left:5px; top: 3px;'><b>Password </b></label>" +
                  $"<input style='display: inline-block; border: 1px solid #ccc; padding: 5px; margin: 3px; position: absolute; right:5px;' type = 'password' name='password' required value='user'></div>" +
                  $"<input type = 'submit' value='Enter' style='height:25px; width: 267px; margin-left: 5px;'></form>";
        }

        [HttpMethod("POST")]
        public string Authenticate(string login, string password)
        {
            var account = accountService.GetAccounts().Find(a => a.Login == login && a.Password == password);
            if (account != null) {
                AccountService.currentAccountID = account.Id;
                AccountService.currentToken = "qwerty";
                if (account.Id == 1)
                    AccountService.currentRole = "Admin";
                else if (account.Id == 2)
                    AccountService.currentRole = "User";

                var d = DateTime.Now.AddDays(15).ToString("r");
                Context.Response.AppendHeader("Set-Cookie", $"token={AccountService.currentToken}; expires={d}; path=/;");
                Context.Response.AppendHeader("Set-Cookie", $"role={AccountService.currentRole}; expires={d}; path =/;");

                return $"<script>window.location = 'http://127.0.0.1:5600/account/home'</script>";
            }
            else {
                return $"<script>window.location = 'http://127.0.0.1:5600/account/login'</script>";
            }
        }

        [HttpMethod("GET")]
        [Authorize("User,Admin")]
        public string Home()
        {
            return $"<h3 style='margin-left:10px;margin-top:15px;margin-bottom:0;'>Home</h3><div style='margin:10px;'>" +
                $"<a href='http://127.0.0.1:5600/toDo/all' style='font-size:15px;color:black;'>My ToDoList</a></div>" +
                $"<div style='margin:10px;'><a href='http://127.0.0.1:5600/account/login' style='font-size:15px;color:black;'>Exit</a></div>";
        }
    }
}
