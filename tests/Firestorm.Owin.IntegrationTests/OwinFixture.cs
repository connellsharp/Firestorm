using System;
using Firestorm.Testing.Http;

namespace Firestorm.Owin.IntegrationTests
{
    public class OwinFixture : IDisposable
    {
        public OwinFixture()
        {
            IntegrationSuite = new OwinItegrationSuite<Startup>(2221);
            IntegrationSuite.Start();
        }
        public IHttpIntegrationSuite IntegrationSuite { get; }

        public void Dispose()
        {
            IntegrationSuite?.Dispose();
        }
    }
}