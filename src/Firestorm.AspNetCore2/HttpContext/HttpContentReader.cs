using System.IO;
using Firestorm.Host.Infrastructure;
using Microsoft.AspNetCore.Http;

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
            HttpRequest request = _httpContext.Request;
            
            if (!request.ContentLength.HasValue && string.IsNullOrEmpty(request.ContentType))
                return null;

            return request.Body;
        }

        public string GetMimeType()
        {
            return _httpContext.Request.ContentType;
        }
    }
}