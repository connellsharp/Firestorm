using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.Client.IntegrationTests
{
    [UsedImplicitly]
    public class DoubleClientIntegrationTests : BasicIntegrationTestsBase
    {
        public DoubleClientIntegrationTests()
            : base(new NetCoreIntegrationSuite<Startup>(Startup.Port))
        { }
    }
}