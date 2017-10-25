using System;
using System.Net.Http;
using Firestorm.Tests.Integration.Base;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.Tests.Integration.AspNetCore
{
    public class NetCoreIntegrationSuite : IHttpIntegrationSuite
    {
        public void Start()
        {
            const string URL = "http://localhost:5000";

            var host = new WebHostBuilder()
                //.UseKestrel()
                .UseUrls(URL)
                .UseStartup<NetCoreIntegrationStartup>()
                .Build();

            host.Start();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(URL)
            };
        }

        public HttpClient HttpClient { get; private set; }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}