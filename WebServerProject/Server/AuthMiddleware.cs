using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebServerProject.Services;

namespace WebServerProject.Server
{
    class AuthMiddleware : IMiddleware
    {
        private readonly HttpDelegate next;

        public AuthMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public void SetCookies(HttpListenerContext context)
        {
            //if (context.Request.RawUrl == "/account/authenticate") {
            //    if (AccountService.currentAccountID != null) {
            //        context.Response.Cookies.Add(new Cookie("token", AccountService.currentToken, "/"));
            //        context.Response.Cookies.Add(new Cookie("role", AccountService.currentRole, "/"));
            //    }
            //}
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            SetCookies(context);

            var cookies = context.Request.Cookies;
            var token = cookies["token"];
            var role = cookies["role"];

            data.Add("IsAuth", token != null ? true : false);
            data.Add("Role", role?.Value ?? "User");

            await next.Invoke(context, data);

            //browser console => document.cookie = "token=qwerty; path=/"
            //                   document.cookie = "role=Admin; path=/"
        }
    }
}
