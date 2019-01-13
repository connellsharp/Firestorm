using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Owin.IntegrationTests
{
    [UsedImplicitly]
    public class OwinTests : BasicIntegrationTestsBase, IClassFixture<OwinFixture>
    {
        public OwinTests(OwinFixture fixture)
            : base(fixture.IntegrationSuite)
        { }
    }
}