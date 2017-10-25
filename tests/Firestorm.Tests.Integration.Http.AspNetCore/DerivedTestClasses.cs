using Firestorm.Tests.Integration.Http.Base.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    [UsedImplicitly]
    public class BasicIntegrationTests : BasicIntegrationTestsBase, IClassFixture<NetCoreIntegrationSuite>
    {
        public BasicIntegrationTests(NetCoreIntegrationSuite integrationSuite)
            : base(integrationSuite)
        { }
    }
}