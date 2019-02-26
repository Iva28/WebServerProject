using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using WebServerProject.Server.Attributes;
using WebServerProject.Services;

namespace WebServerProject.Server
{
    public class MvcMiddleware : IMiddleware
    {
        private HttpDelegate next;

        public MvcMiddleware(HttpDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpListenerContext context, Dictionary<string, object> data)
        {
            HttpListenerResponse response = context.Response;

            StreamWriter writer = new StreamWriter(response.OutputStream);
            try
            {
                string resp = FindControllerAction(context , data);
                if (resp != null) {
                    response.ContentType = "text/html";
                    writer.Write(resp);
                }
                else {
                    await next.Invoke(context, data);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ContentType = "text/plain";
                writer.Write(ex.Message);
            }
            finally
            {
                writer.Close();
            }            
        }

        private string FindControllerAction(HttpListenerContext context, Dictionary<string, object> data)           
        {
            HttpListenerRequest request = context.Request;

            string[] urlparts = request.Url.PathAndQuery.Split(new char[] { '/', '\\', '?' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlparts.Length < 2) return null;

            string controller = urlparts[0];
            string action = urlparts[1];

            Assembly curAssembly = Assembly.GetExecutingAssembly();
            Type controllerType = curAssembly.GetType($"WebServerProject.Controllers.{controller}Controller", false, true);
            if (controllerType == null)
                return null;



            MethodInfo actionMethod = controllerType.GetMethod(action, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (actionMethod == null)
                return null;

            var attr = actionMethod.GetCustomAttribute<HttpMethodAttribute>();
            if (attr != null)
            {
                if (String.Compare(attr.Method, request.HttpMethod, true) != 0)
                {
                    Console.WriteLine($"{attr.Method} - {request.HttpMethod}");
                    return null;
                }
            }


            var authAttr = actionMethod.GetCustomAttribute<AuthorizeAttribute>();
            if (authAttr != null)
            {
                if ((bool)data["IsAuth"] == false)
                {
                    return "HTTP ERROR 401: Not authorized";
                }
                if (authAttr.Roles != null)
                {
                    var roles = authAttr.Roles.Split(',');
                    if (!roles.Contains(data["Role"]))
                    {
                        return "HTTP ERROR 401: Access Denied";
                    }
                }
            }

            List<object> paramsToMethod = new List<object>();
            NameValueCollection coll = null;

            if (request.HttpMethod == "GET")
            {
                if (urlparts.Length == 2 && actionMethod.GetParameters().Length != 0) return null;
                if (urlparts.Length > 2)
                {
                    coll = System.Web.HttpUtility.ParseQueryString(urlparts[2]);
                }
            }
            else if (request.HttpMethod == "POST")
            {
                string body;
                using (StreamReader reader = new StreamReader(request.InputStream))
                {
                    body = reader.ReadToEnd();
                }
                coll = System.Web.HttpUtility.ParseQueryString(body);
            }
            else { return null; }

            ParameterInfo[] parameters = actionMethod.GetParameters();
            foreach (ParameterInfo pi in parameters)
            {
                paramsToMethod.Add(Convert.ChangeType(coll[pi.Name], pi.ParameterType));
            }
            if (paramsToMethod.Count != actionMethod.GetParameters().Length) return null;

            var _this = MyWebServer.IOC.Resolve(controllerType);

            // p.SetMethod?.IsPublic ?? false)
            // (p.SetMethod != null ? p.SetMethod.IsPublic : null) != null ? p.SetMethod.IsPublic : false;
            var propHttpContext = _this.GetType().GetProperties().Where(p => p.PropertyType == typeof(HttpListenerContext) && (p.SetMethod?.IsPublic ?? false)).FirstOrDefault();
            if (propHttpContext != null)
            {
                propHttpContext.SetValue(_this, context);
            }

            var args = paramsToMethod.ToArray();
            var res = actionMethod.Invoke(_this, args);

            return res as string;
        }
    }
}
