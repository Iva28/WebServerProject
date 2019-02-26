using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebServerProject.Server
{
    class HttpContextProvider : IHttpContextProvider
    {
        private HttpListenerContext _context;

        public HttpContextProvider(MyWebServer server)
        {
            _context = server.Context;
        }

        public HttpListenerContext GetHttpContext()
        {
            return _context;
        }
    }
}
