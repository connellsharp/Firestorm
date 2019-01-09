using System;
using System.IO;
using System.Net.Http;
using Firestorm.FunctionalTests.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.FunctionalTests.Setup
{
    public class HostClientPair : IDisposable
    {      
        private static int _startPort = 2230;

        private readonly IWebHost _host;

        public HostClientPair(IStartupConfigurer config)
        {
            var url = "http://localhost:" + _startPort++;

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .ConfigureServices(s => s.AddSingleton(config))
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            _host.Start();
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            _host.Dispose();
            HttpClient.Dispose();
        }
    }
}