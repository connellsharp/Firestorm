using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using Firestorm.Endpoints.Query;

namespace Firestorm.Endpoints.WebApi2
{
    public class HttpRequestRestEndpointContext : IRestEndpointContext
    {
        private readonly HttpRequestMessage _request;

        public HttpRequestRestEndpointContext(HttpRequestContext requestContext, HttpRequestMessage request, RestEndpointConfiguration configuration)
        {
            User = new PrincipalUser(requestContext.Principal);
            _request = request;
            Configuration = configuration;
        }

        public RestEndpointConfiguration Configuration { get; }

        public IRestUser User { get; }

        public IRestCollectionQuery GetQuery()
        {
            string queryString = _request.RequestUri.Query.TrimStart('?');
            return new QueryStringCollectionQuery(Configuration.QueryStringConfiguration, queryString);
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}