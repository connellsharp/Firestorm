using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web.Defaults;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinRestEndpointContext : IRestEndpointContext
    {
        public OwinRestEndpointContext(IOwinContext owinContext, RestEndpointConfiguration configuration)
        {
            Configuration = configuration;
            User = new PrincipalUser(owinContext.Request.User);
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