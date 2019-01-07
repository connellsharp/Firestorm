using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.AspNetWebApi2.IntegrationTests
{
    [UsedImplicitly]
    public class WebApiTests : BasicIntegrationTestsBase
    {
        public WebApiTests()
            : base(new OwinItegrationSuite<Startup>(1112))
        { }
    }
}