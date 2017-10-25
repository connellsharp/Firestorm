using System;
using System.Net.Http;
using Firestorm.Tests.Integration.Http.Base;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    public class NetCoreIntegrationSuite : IHttpIntegrationSuite
    {
        private IWebHost _host;

        public void Start()
        {
            const string URL = "http://localhost:5000";

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(URL)
                .UseStartup<NetCoreIntegrationStartup>()
                .Build();

            _host.Start();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(URL)
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