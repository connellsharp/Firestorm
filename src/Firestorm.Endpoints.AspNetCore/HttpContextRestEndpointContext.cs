using System;
using Firestorm.Core;
using Firestorm.Endpoints.Query;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Endpoints.AspNetCore
{
    internal class HttpContextRestEndpointContext : IRestEndpointContext
    {
        private readonly HttpContext _httpContext;

        public HttpContextRestEndpointContext(HttpContext httpContext, RestEndpointConfiguration configuration)
        {
            _httpContext = httpContext;
            Configuration = configuration;
            User = new PrincipalUser(httpContext.User);
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; }

        public IRestCollectionQuery GetQuery()
        {
            return new QueryStringCollectionQuery(Configuration.QueryStringConfiguration, _httpContext.Request.QueryString.Value);
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}