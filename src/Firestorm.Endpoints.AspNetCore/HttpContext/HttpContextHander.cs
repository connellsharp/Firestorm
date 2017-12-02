using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Start;

namespace Firestorm.Endpoints.AspNetCore.HttpContext
{
    internal class HttpContextHander : IHttpRequestHandler
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContextHander(Microsoft.AspNetCore.Http.HttpContext httpContext)
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

        public async Task SetResponseBody(object obj)
        {
            var negotator = new ContentNegotiator(new HttpContentWriter(_httpContext));
            await negotator.SetResponseBody(obj);
        }

        public ResourceBody GetRequestBodyObject()
        {
            return ResourceBodyJsonConverter.ReadResourceStream(_httpContext.Request.Body);
        }

        public void SetResponseHeader(string key, string value)
        {
            _httpContext.Response.Headers[key] = value;
        }
    }
}