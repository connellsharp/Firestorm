using Firestorm.Tests.Integration.Http.Base.Tests;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Http.AspNetCore
{
    [UsedImplicitly]
    public class NetCoreOriginalConfigTests : BasicIntegrationTestsBase
    {
        public NetCoreOriginalConfigTests()
            : base(new NetCoreIntegrationSuite<NetCoreOriginalConfigStartup>(2222))
        { }
    }

    [UsedImplicitly]
    public class NetCoreServicesOptionsTests : BasicIntegrationTestsBase
    {
        public NetCoreServicesOptionsTests()
            : base(new NetCoreIntegrationSuite<NetCoreServicesOptionsStartup>(2223))
        { }
    }

    [UsedImplicitly]
    public class NetCoreServicesSingletonsTests : BasicIntegrationTestsBase
    {
        public NetCoreServicesSingletonsTests()
            : base(new NetCoreIntegrationSuite<NetCoreServicesSingletonsStartup>(2224))
        { }
    }
}