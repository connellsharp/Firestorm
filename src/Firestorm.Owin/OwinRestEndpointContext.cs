using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Web.Defaults;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinRestEndpointContext : IRestEndpointContext
    {
        private readonly IOwinContext _owinContext;

        public OwinRestEndpointContext(IOwinContext owinContext, RestEndpointConfiguration configuration)
        {
            _owinContext = owinContext;
            Configuration = configuration;
            User = new PrincipalUser(owinContext.Request.User);
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; }

        public IRestCollectionQuery GetQuery()
        {
            return new QueryStringCollectionQuery(Configuration.QueryStringConfiguration, _owinContext.Request.QueryString.Value);
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}