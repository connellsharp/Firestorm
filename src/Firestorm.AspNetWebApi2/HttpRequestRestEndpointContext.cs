using System;
using System.Web.Http.Controllers;
using Firestorm.Endpoints;

namespace Firestorm.AspNetWebApi2
{
    public class HttpRequestRestEndpointContext : IRestEndpointContext
    {
        public HttpRequestRestEndpointContext(HttpRequestContext requestContext, RestEndpointConfiguration configuration)
        {
            User = new PrincipalUser(requestContext.Principal);
            Configuration = configuration;
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