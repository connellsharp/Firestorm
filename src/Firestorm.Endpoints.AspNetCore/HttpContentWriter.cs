using System.Threading.Tasks;
using Firestorm.Endpoints.Formatting;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Endpoints.AspNetCore
{
    public class HttpContentWriter : IContentWriter
    {
        private readonly HttpContext _httpContext;

        public HttpContentWriter(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetMimeType(string mimeType)
        {
            _httpContext.Response.ContentType = mimeType;
        }

        public void SetLength(int bytesLength)
        {
            _httpContext.Response.ContentLength = bytesLength;
        }

        public Task WriteBytesAsync(byte[] bytes)
        {
            return _httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length, _httpContext.RequestAborted);
        }
    }
}