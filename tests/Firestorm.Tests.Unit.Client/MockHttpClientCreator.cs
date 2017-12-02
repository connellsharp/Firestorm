using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Firestorm.Client;

namespace Firestorm.Tests.Client
{
    internal class MockHttpClientCreator : IHttpClientCreator
    {
        public const string BaseUrl = "http://localhost/base/";

        public HttpClient Create()
        {
            return new HttpClient(new MockHttpMessageHandler(this))
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public HttpRequestMessage LastRequest { get; private set; }

        public string LastRequestBody { get; private set; }

        public HttpStatusCode ResponseStatusCode { private get; set; } = HttpStatusCode.OK;

        public string ResponseBody { private get; set; } = "{}";

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly MockHttpClientCreator _parentCreator;

            public MockHttpMessageHandler(MockHttpClientCreator parentCreator)
            {
                _parentCreator = parentCreator;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _parentCreator.LastRequest = request;

                _parentCreator.LastRequestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : null;

                var response = new HttpResponseMessage
                {
                    StatusCode = _parentCreator.ResponseStatusCode,
                    Content = new StringContent(_parentCreator.ResponseBody)
                };

                return response;
            }
        }
    }
}