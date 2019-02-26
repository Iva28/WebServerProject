using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace WebServerProject.Server
{
    class AuthMiddleware : IMiddleware
    {
        private readonly HttpDelegate next;

        public AuthMiddleware(HttpDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            var token = context.Request.Cookies["token"];
            var role = context.Request.Cookies["role"];

            data.Add("IsAuth", token != null ? true : false);
            data.Add("Role", role?.Value ?? "User");
            await next.Invoke(context, data);

            //browser console => document.cookie = "token=qwerty; role=Admin; path=/;";
        }
    }
}
