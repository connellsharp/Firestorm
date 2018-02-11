using System.Net;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Start;

namespace Firestorm.AspNetCore2.HttpContext
{
    internal class HttpContextHandler : IHttpRequestHandler
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContextHandler(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public string RequestMethod => _httpContext.Request.Method;

        public string ResourcePath => _httpContext.Request.Path.Value;

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _httpContext.Response.StatusCode = (int) statusCode;
        }

        public IPreconditions GetPreconditions()
        {
            return new HttpPreconditions(_httpContext.Request.Headers);
        }

        public IContentReader GetContentReader()
        {
            return new HttpContentReader(_httpContext);
        }

        public void SetResponseHeader(string key, string value)
        {
            _httpContext.Response.Headers[key] = value;
        }

        public IContentAccepts GetAcceptHeaders()
        {
            return new HttpContentAccepts();
        }

        public IContentWriter GetContentWriter()
        {
            return new HttpContentWriter(_httpContext);
        }
    }

    internal class HttpContentAccepts : IContentAccepts
    { }
}