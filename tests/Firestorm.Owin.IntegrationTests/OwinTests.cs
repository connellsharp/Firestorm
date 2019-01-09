using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.Owin.IntegrationTests
{
    [UsedImplicitly]
    public class OwinTests : BasicIntegrationTestsBase
    {
        public OwinTests()
            : base(new OwinItegrationSuite<Startup>(2221))
        { }
    }
}