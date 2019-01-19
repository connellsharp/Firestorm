using System;
using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class StandardStartupIntegratedTests : BasicIntegrationTestsBase, IClassFixture<IntegratedFixture>
    {
        public StandardStartupIntegratedTests(IntegratedFixture fixture)
            : base(fixture.IntegrationSuite)
        { }
    }
    
    public class IntegratedFixture : IDisposable
    {
        public IntegratedFixture()
        {
            IntegrationSuite = new IntegratedIntegrationSuite<StandardStartup>();
            IntegrationSuite.Start();
        }

        internal IntegratedIntegrationSuite<StandardStartup> IntegrationSuite { get; }

        public void Dispose()
        {
            IntegrationSuite?.Dispose();
        }
    }
}