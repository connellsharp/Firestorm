using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class StandardStartupTests : BasicIntegrationTestsBase, IClassFixture<StandardFixture>
    {
        public StandardStartupTests(StandardFixture fixture)
            : base(fixture.IntegrationSuite)
        { }
    }

    public class StandardFixture : HttpFixture<OptionsStartup>
    {
        public StandardFixture()
            : base(2223)
        {
        }
    }
}