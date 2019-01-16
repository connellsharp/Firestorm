using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetWebApi2.IntegrationTests
{
    [UsedImplicitly]
    public class WebApiTests : BasicIntegrationTestsBase, IClassFixture<WebApiFixture>
    {
        public WebApiTests(WebApiFixture fixture)
            : base(fixture.IntegrationSuite)
        { }
    }
}