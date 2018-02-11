using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Formatting;
using Firestorm.Endpoints.Preconditions;
using Firestorm.Endpoints.Start;

namespace Firestorm.Tests.Unit.Endpoints.Start
{
    internal class MockHttpRequestHandler : IHttpRequestHandler, IContentWriter
    {
        public string RequestMethod { get; internal set; }

        public string ResourcePath { get; internal set; }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            ResponseStatusCode = statusCode;
        }

        public HttpStatusCode ResponseStatusCode { get; set; }

        public IPreconditions GetPreconditions()
        {
            return null;
        }

        public IContentReader GetContentReader()
        {
            return RequestContentReader;
        }

        public IContentReader RequestContentReader { get; set; }

        public Dictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();

        public void SetResponseHeader(string key, string value)
        {
            ResponseHeaders.Add(key, value);
        }

        public IContentAccepts GetAcceptHeaders()
        {
            return null;
        }

        public IContentWriter ResponseContentWriter { get; set; }

        public IContentWriter GetContentWriter()
        {
            return this;
        }

        public void SetMimeType(string mimeType)
        {
        }

        public void SetLength(int bytesLength)
        {
        }

        public Task WriteBytesAsync(byte[] bytes)
        {
            return Task.CompletedTask;
        }
    }
}