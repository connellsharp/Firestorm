using System;
using Firestorm.Testing.Http;

namespace Firestorm.AspNetWebApi2.IntegrationTests
{
    public class WebApiFixture : IDisposable
    {
        public WebApiFixture()
        {
            IntegrationSuite = new OwinItegrationSuite<Startup>(2222);
            IntegrationSuite.Start();
        }
        public IHttpIntegrationSuite IntegrationSuite { get; }

        public void Dispose()
        {
            IntegrationSuite?.Dispose();
        }
    }
}