﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Core.Web;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Formatting.Json;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Responses;
using Firestorm.Endpoints.Start;
using Microsoft.Owin;

namespace Firestorm.Owin
{
    public class OwinContextHandler : IHttpRequestHandler
    {
        private readonly IOwinContext _owinContext;

        public OwinContextHandler(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public string RequestMethod => _owinContext.Request.Method;

        public string ResourcePath => _owinContext.Request.Path.Value;

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            _owinContext.Response.StatusCode = (int) statusCode;
        }

        public IPreconditions GetPreconditions()
        {
            return new OwinPreconditions(_owinContext);
        }

        public Task SetResponseBodyAsync(object obj)
        {
            var negotator = new ContentNegotiator(new OwinContentAccepts(), new OwinContentWriter(_owinContext));
            return negotator.SetResponseBodyAsync(obj);
        }

        public ResourceBody GetRequestBodyObject()
        {
            var reader = new ContentReader();
            return reader.ReadResourceStream(_owinContext.Request.Body);
        }

        public void SetResponseHeader(string key, string value)
        {
            _owinContext.Response.Headers[key] = value;
        }
    }

    public class OwinContentAccepts : IContentAccepts
    { }
}