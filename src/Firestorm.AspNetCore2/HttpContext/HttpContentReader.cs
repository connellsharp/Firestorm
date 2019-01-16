using System.IO;
using Firestorm.Host.Infrastructure;

namespace Firestorm.AspNetCore2.HttpContext
{
    internal class HttpContentReader : IContentReader
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;

        public HttpContentReader(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public Stream GetContentStream()
        {
            if (!_httpContext.Request.ContentLength.HasValue)
                return null;

            return _httpContext.Request.Body;
        }

        public string GetMimeType()
        {
            return _httpContext.Request.ContentType;
        }
    }
}