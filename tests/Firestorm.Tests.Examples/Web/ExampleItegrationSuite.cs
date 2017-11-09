using System;
using System.Net.Http;
using Firestorm.Endpoints;
using Firestorm.Endpoints.Owin;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.Integration.Http.Base;
using Microsoft.Owin.Hosting;
using Owin;

namespace Firestorm.Tests.Examples.Web
{
    public class ExampleItegrationSuite : IHttpIntegrationSuite
    {
        private static readonly Random Random = new Random();
        private readonly string _url = "http://localhost:" + Random.Next(1200, 1500);
        private readonly FirestormConfiguration _firestormConfig;

        public ExampleItegrationSuite(FirestormConfiguration firestormConfig)
        {
            _firestormConfig = firestormConfig;
        }

        public void Start()
        {
            WebApplication = WebApp.Start(_url, delegate(IAppBuilder app)
            {
                app.Use<SpoofUserMiddleware>();
                app.UseFirestorm(_firestormConfig);
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