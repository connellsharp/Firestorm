using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Start;

namespace Firestorm.Endpoints.AspNetCore.HttpContext
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

        public Task SetResponseBodyAsync(object obj)
        {
            var negotiator = new ContentNegotiator(new HttpContentAccepts(), new HttpContentWriter(_httpContext));
            return negotiator.SetResponseBodyAsync(obj);
        }

        public ResourceBody GetRequestBodyObject()
        {
            var reader = new ContentReader();
            return reader.ReadResourceStream(_httpContext.Request.Body);
        }

        public void SetResponseHeader(string key, string value)
        {
            _httpContext.Response.Headers[key] = value;
        }
    }

    internal class HttpContentAccepts : IContentAccepts
    { }
}