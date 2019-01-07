using System;
using System.Web.Http.Controllers;
using Firestorm.Host;
using Firestorm.Host.Infrastructure;

namespace Firestorm.AspNetWebApi2
{
    public class WebApiRequestContext : IRequestContext
    {
        public WebApiRequestContext(HttpRequestContext requestContext)
        {
            User = new PrincipalUser(requestContext.Principal);
        }

        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}