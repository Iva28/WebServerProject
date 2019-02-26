using System;

namespace WebServerProject.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class HttpMethodAttribute : Attribute
    {
        public HttpMethodAttribute(string method)
        {
            if (String.Compare(method, "get", true)!= 0  && String.Compare(method, "post", true) != 0)
            {
                throw new ArgumentException($"{nameof(method)} must be GET or POST");
            }
            this.Method = method;
        }

        public string Method { get; private set; }
    }
}
