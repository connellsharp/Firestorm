using Firestorm.Testing.Http.Tests;
using Firestorm.Tests.Integration.Http.NetFramework.Web;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    [UsedImplicitly]
    public class PureOwinBasicIntegrationTests : BasicIntegrationTestsBase
    {
        public PureOwinBasicIntegrationTests()
            : base(new NetFrameworkItegrationSuite<PureOwinExampleStartup>(1111))
        { }
    }

    [UsedImplicitly]
    public class WebApiBasicIntegrationTests : BasicIntegrationTestsBase
    {
        public WebApiBasicIntegrationTests()
            : base(new NetFrameworkItegrationSuite<WebApiExampleStartup>(1112))
        { }
    }

    [UsedImplicitly]
    public class DoubleClientBasicIntegrationTests : BasicIntegrationTestsBase
    {
        public DoubleClientBasicIntegrationTests()
            : base(new NetFrameworkItegrationSuite<DoubleClientExampleStartup>(DoubleClientExampleStartup.Port))
        { }
    }
}