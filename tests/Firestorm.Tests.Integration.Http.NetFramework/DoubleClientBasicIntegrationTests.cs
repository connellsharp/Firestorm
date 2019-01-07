using Firestorm.Testing.Http.Tests;
using Firestorm.Tests.Integration.Http.NetFramework.Web;
using JetBrains.Annotations;

namespace Firestorm.Tests.Integration.Http.NetFramework
{
    [UsedImplicitly]
    public class DoubleClientBasicIntegrationTests : BasicIntegrationTestsBase
    {
        public DoubleClientBasicIntegrationTests()
            : base(new NetFrameworkItegrationSuite<DoubleClientExampleStartup>(DoubleClientExampleStartup.Port))
        { }
    }
}