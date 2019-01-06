using Firestorm.Tests.Integration.Http.Base.Tests;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    [UsedImplicitly]
    public class NetCoreServicesTests : BasicIntegrationTestsBase
    {
        public NetCoreServicesTests()
            : base(new NetCoreIntegrationSuite<NetCoreServicesStartup>(2223))
        { }
    }

    [UsedImplicitly]
    public class NetCoreServicesWithOptionsStartupTests : BasicIntegrationTestsBase
    {
        public NetCoreServicesWithOptionsStartupTests()
            : base(new NetCoreIntegrationSuite<NetCoreServicesWithOptionsStartup>(2224))
        { }
    }
}