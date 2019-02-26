using System;

namespace WebServerProject.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute()
        {
            this.Roles = null;
        }

        public AuthorizeAttribute(string roles)
        {
            this.Roles = roles;
        }

        public string Roles { get; set; }
    }
}
