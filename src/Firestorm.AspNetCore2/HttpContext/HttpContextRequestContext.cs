using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web.Defaults;
using Firestorm.Host;

namespace Firestorm.AspNetCore2.HttpContext
{
    using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

    internal class HttpContextRequestContext : IRequestContext
    {
        public HttpContextRequestContext(HttpContext httpContext)
        {
            User = new PrincipalUser(httpContext.User);
        }

        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}