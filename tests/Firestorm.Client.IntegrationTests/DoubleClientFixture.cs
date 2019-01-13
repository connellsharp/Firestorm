using System;
using Firestorm.Testing.Http;

namespace Firestorm.Client.IntegrationTests
{
    public class DoubleClientFixture : IDisposable
    {
        public DoubleClientFixture()
        {
            IntegrationSuite = new NetCoreIntegrationSuite<Startup>(Startup.Port);
            IntegrationSuite.Start();
        }
        public IHttpIntegrationSuite IntegrationSuite { get; }

        public void Dispose()
        {
            IntegrationSuite?.Dispose();
        }
    }
}