using System.Net;
using Firestorm.Host.Infrastructure;

namespace Firestorm.AspNetCore2.HttpContext
{
    internal class HttpContextReader : IHttpRequestReader
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContextReader(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string RequestMethod => _httpContext.Request.Method;

        public string ResourcePath => _httpContext.Request.Path.Value;

        public IPreconditions GetPreconditions()
        {
            return new HttpPreconditions(_httpContext.Request.Headers);
        }

        public IContentReader GetContentReader()
        {
            return new HttpContentReader(_httpContext);
        }

        public string GetQueryString()
        {
            return _httpContext.Request.QueryString.Value;
        }
    }
}