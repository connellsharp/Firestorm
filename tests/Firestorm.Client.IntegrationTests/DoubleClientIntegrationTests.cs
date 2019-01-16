using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;
using Xunit;

namespace Firestorm.Client.IntegrationTests
{
    [UsedImplicitly]
    public class DoubleClientIntegrationTests : BasicIntegrationTestsBase, IClassFixture<DoubleClientFixture>
    {
        public DoubleClientIntegrationTests(DoubleClientFixture fixture)
            : base(fixture.IntegrationSuite)
        { }
    }
}