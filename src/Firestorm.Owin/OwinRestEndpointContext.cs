using System;
using Firestorm.Core;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Query;
using Firestorm.Endpoints.Start;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinRestEndpointContext : IRestEndpointContext
    {
        private readonly IOwinContext _owinContext;
        private readonly FirestormConfiguration _configuration;

        public OwinRestEndpointContext(IOwinContext owinContext, FirestormConfiguration configuration)
        {
            _owinContext = owinContext;
            _configuration = configuration;
            User = new PrincipalUser(owinContext.Request.User);
        }

        RestEndpointConfiguration IRestEndpointContext.Configuration => _configuration.EndpointConfiguration;

        public IRestUser User { get; }

        public IRestCollectionQuery GetQuery()
        {
            return new QueryStringCollectionQuery(_configuration.EndpointConfiguration.QueryStringConfiguration, _owinContext.Request.QueryString.Value);
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}