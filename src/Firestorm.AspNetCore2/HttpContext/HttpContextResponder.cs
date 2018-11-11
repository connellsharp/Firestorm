using System.Net;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Web;

namespace Firestorm.AspNetCore2.HttpContext
{
    internal class HttpContextResponder : IHttpRequestResponder
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContextResponder(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _httpContext.Response.StatusCode = (int) statusCode;
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
}