using System;
using Firestorm.Testing.Http;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    public abstract class HttpFixture<TStartup> : IDisposable
        where TStartup : class
    {
        protected HttpFixture(int port)
        {
            IntegrationSuite = new KestrelIntegrationSuite<TStartup>(port);
            IntegrationSuite.Start();
        }

        public IHttpIntegrationSuite IntegrationSuite { get; }

        public void Dispose()
        {
            IntegrationSuite?.Dispose();
        }
    }
}