using System.Net.Http;
using Firestorm.Testing.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    internal class IntegratedIntegrationSuite<TStartup> : IHttpIntegrationSuite 
        where TStartup : class
    {
        private readonly WebApplicationFactory<TStartup> _appFactory;

        public IntegratedIntegrationSuite()
        {
            _appFactory = new ExtendedWebApplicationFactory();
        }

        public void Start()
        {
            HttpClient = _appFactory.CreateClient();
        }

        public HttpClient HttpClient { get; private set; }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        private class ExtendedWebApplicationFactory : WebApplicationFactory<TStartup> 
        {
            protected override IWebHostBuilder CreateWebHostBuilder()
            {
                return new WebHostBuilder()
                    .UseStartup<TStartup>();
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.UseContentRoot(".");
                base.ConfigureWebHost(builder);
            }
        }
    }
}