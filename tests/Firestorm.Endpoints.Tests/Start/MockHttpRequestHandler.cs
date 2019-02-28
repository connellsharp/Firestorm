﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Firestorm.Endpoints.Tests.Start.Readers;
using Firestorm.Host.Infrastructure;

namespace Firestorm.Endpoints.Tests.Start
{
    internal class MockHttpRequestHandler : IHttpRequestReader, IHttpRequestResponder, IContentWriter
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

        public string GetQueryString()
        {
            return null;
        }

        public IContentReader RequestContentReader { get; set; } = new EmptyReader();

        public Dictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();

        public void SetResponseHeader(string key, string value)
        {
            ResponseHeaders.Add(key, value);
        }

        public IContentAccepts GetAcceptHeaders()
        {
            return null;
        }

        public IContentWriter GetContentWriter()
        {
            return this;
        }

        public void SetMimeType(string mimeType)
        {
            // Not required for these tests
        }

        public void SetLength(int bytesLength)
        {
            // Not required for these tests
        }

        public Task WriteBytesAsync(byte[] bytes)
        {
            return Task.CompletedTask;
        }
    }
}