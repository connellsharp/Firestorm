using System;
using System.IO;
using System.Net.Http;
using Firestorm.FunctionalTests.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.FunctionalTests.Tests.Setup
{
    public class HostClientPair : IDisposable
    {
        private readonly IWebHost _host;

        public HostClientPair(int port, FirestormApiTech tech)
        {
            var url = "http://localhost:" + port;

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseIISIntegration()
                .ConfigureServices(s => s.AddSingleton(new StartupTechSettings(tech)))
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

    public class StartupTechSettings
    {
        public FirestormApiTech Tech { get; }

        public StartupTechSettings(FirestormApiTech tech)
        {
            Tech = tech;
        }
    }
}