using System.Net.Http.Headers;
using Firestorm.Endpoints.Preconditions;

namespace Firestorm.Endpoints.WebApi2
{
    internal class WebApiPreconditions : IPreconditions
    {
        private readonly HttpRequestHeaders _requestHeaders;

        public WebApiPreconditions(HttpRequestHeaders requestHeaders)
        {
            _requestHeaders = requestHeaders;
        }

        public string IfMatch => _requestHeaders.IfMatch.ToString();

        public string IfNoneMatch => _requestHeaders.IfNoneMatch.ToString();

        public string IfModifiedSince => _requestHeaders.IfModifiedSince.ToString();

        public string IfUnmodifiedSince => _requestHeaders.IfUnmodifiedSince.ToString();

        public string IfRange => _requestHeaders.IfRange.ToString();
    }
}