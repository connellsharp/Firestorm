using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.AspNetWebApi2.IntegrationTests
{
    [UsedImplicitly]
    public class WebApiBasicIntegrationTests : BasicIntegrationTestsBase
    {
        public WebApiBasicIntegrationTests()
            : base(new NetFrameworkItegrationSuite<Startup>(1112))
        { }
    }
}