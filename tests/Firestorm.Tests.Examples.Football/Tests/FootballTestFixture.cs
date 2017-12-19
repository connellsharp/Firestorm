using System;
using System.IO;
using System.Net.Http;
using Firestorm.Tests.Examples.Football.Web;
using Microsoft.AspNetCore.Hosting;

namespace Firestorm.Tests.Examples.Football.Tests
{
    public class FootballTestFixture : IDisposable
    {
        private readonly IWebHost _host;

        public HttpClient HttpClient { get; }

        public FootballTestFixture()
        {
            var url = "http://localhost:1337";

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            _host.Start();
        }

        public void Dispose()
        {
            _host.Dispose();
            HttpClient.Dispose();
        }
    }
}