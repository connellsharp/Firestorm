using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web.Defaults;

namespace Firestorm.AspNetCore2.HttpContext
{
    using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

    internal class HttpContextRestEndpointContext : IRestEndpointContext
    {
        public HttpContextRestEndpointContext(HttpContext httpContext, RestEndpointConfiguration configuration)
        {
            Configuration = configuration;
            User = new PrincipalUser(httpContext.User);
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}