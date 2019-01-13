using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class OptionsStartupTests : BasicIntegrationTestsBase, IClassFixture<OptionsFixture>
    {
        public OptionsStartupTests(OptionsFixture fixture)
            : base(fixture.IntegrationSuite)
        {
        }
    }

    public class OptionsFixture : HttpFixture<OptionsStartup>
    {
        public OptionsFixture()
            : base(2224)
        {
        }
    }
}