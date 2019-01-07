using System;
using System.Net.Http;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Web;
using Firestorm.EntityFramework6;
using Firestorm.Extensions.AspNetCore;
using Firestorm.Owin;
using Firestorm.Stems;
using Firestorm.Stems.Roots;
using Firestorm.Tests.Examples.Music.Data;
using Firestorm.Testing.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace Firestorm.Tests.Examples.Music.Web
{
    public class ExampleItegrationSuite : IHttpIntegrationSuite
    {
        private readonly RestEndpointConfiguration _config;
        private readonly Type _testClassType;

        private static readonly Random Random = new Random();
        private readonly string _url = "http://localhost:" + Random.Next(1200, 1500);

        public ExampleItegrationSuite(RestEndpointConfiguration config, Type testClassType)
        {
            _config = config;
            _testClassType = testClassType;
        }

        public void Start()
        {
            WebApplication = WebApp.Start(_url, delegate(IAppBuilder app)
            {
                app.Use<SpoofUserMiddleware>();

                app.UseFirestorm(c => c
                    .AddEndpoints(_config)
                    .AddStems()
                    .Add<ITypeGetter>(new NestedTypeGetter(_testClassType))
                    .AddDataSource(new EntitiesDataSource<ExampleDataContext>())
                );
            });

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };
        }

        public IDisposable WebApplication { get; private set; }

        public HttpClient HttpClient { get; private set; }

        public void Dispose()
        {
            WebApplication?.Dispose();
            HttpClient?.Dispose();
        }
    }
}