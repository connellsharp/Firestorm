using System;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Query;

namespace Firestorm.AspNetCore2.HttpContext
{
    internal class HttpContextRestEndpointContext : IRestEndpointContext
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContextRestEndpointContext(Microsoft.AspNetCore.Http.HttpContext httpContext, RestEndpointConfiguration configuration)
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