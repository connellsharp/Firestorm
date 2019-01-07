using System;
using System.Net.Http;
using Firestorm.Testing.Http;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    internal class KestrelIntegrationSuite<TStartup> : IHttpIntegrationSuite
        where TStartup : class
    {
        private readonly int _portNumber;
        private IWebHost _host;

        public KestrelIntegrationSuite(int portNumber)
        {
            _portNumber = portNumber;
        }

        public void Start()
        {
            string url = "http://localhost:" + _portNumber;

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(url)
                .UseStartup<TStartup>()
                .Build();

            _host.Start();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
        }

        public HttpClient HttpClient { get; private set; }

        public void Dispose()
        {
            _host?.Dispose();
            HttpClient?.Dispose();
        }
    }
}