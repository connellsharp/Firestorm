using Firestorm.Host.Infrastructure;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    internal class OwinPreconditions : IPreconditions
    {
        private readonly IHeaderDictionary _requestHeaders;

        public OwinPreconditions(IOwinContext owinContext)
        {
            _requestHeaders = owinContext.Request.Headers;
        }

        public string IfMatch => _requestHeaders["If-Match"];

        public string IfNoneMatch => _requestHeaders["If-None-Match"];

        public string IfModifiedSince => _requestHeaders["If-Modified-Since"];

        public string IfUnmodifiedSince => _requestHeaders["If-Unmodified-Since"];

        public string IfRange => _requestHeaders["If-Range"];
    }
}