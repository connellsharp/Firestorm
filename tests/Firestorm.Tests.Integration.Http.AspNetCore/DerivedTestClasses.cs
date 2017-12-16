using Firestorm.Tests.Integration.Http.Base.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    [UsedImplicitly]
    public class AspNetCoreBasicIntegrationTests : BasicIntegrationTestsBase, IClassFixture<NetCoreIntegrationSuite>
    {
        public AspNetCoreBasicIntegrationTests(NetCoreIntegrationSuite integrationSuite)
            : base(integrationSuite)
        { }
    }
}