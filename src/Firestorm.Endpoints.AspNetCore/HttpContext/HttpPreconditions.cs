using Firestorm.Endpoints.Preconditions;
using Microsoft.AspNetCore.Http;

namespace Firestorm.Endpoints.AspNetCore
{
    internal class HttpPreconditions : IPreconditions
    {
        private readonly IHeaderDictionary _headerDictionary;

        public HttpPreconditions(IHeaderDictionary headerDictionary)
        {
            _headerDictionary = headerDictionary;
        }

        public string IfMatch => _headerDictionary["If-Match"];
        public string IfNoneMatch => _headerDictionary["If-None-Match"];
        public string IfModifiedSince => _headerDictionary["If-Modified-Since"];
        public string IfUnmodifiedSince => _headerDictionary["If-Unmodified-Since"];
        public string IfRange => _headerDictionary["If-Range"];
    }
}