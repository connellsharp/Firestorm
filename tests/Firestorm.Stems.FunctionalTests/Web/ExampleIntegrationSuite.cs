using System;
using System.Net.Http;
using Firestorm.Testing.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Firestorm.Stems.FunctionalTests.Web
{
    public class ExampleIntegrationSuite : IHttpIntegrationSuite
    {
        private readonly Type _testClassType;

        private static int _startPort = 2240;
        private readonly string _url = "http://localhost:" + _startPort++;

        private IWebHost _host;

        public ExampleIntegrationSuite(Type testClassType)
        {
            _testClassType = testClassType;
        }

        public void Start()
        {
            var config = new FunctionalTestConfig
            {
                TestClassType = _testClassType
            };
            
            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(_url)
                .ConfigureServices(s => s.AddSingleton(config))
                .UseStartup<Startup>()
                .Build();

            _host.Start();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(_url)
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