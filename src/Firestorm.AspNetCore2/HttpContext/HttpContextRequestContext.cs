using System;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

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