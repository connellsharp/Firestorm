using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.Owin.IntegrationTests
{
    [UsedImplicitly]
    public class OwinIntegrationTests : BasicIntegrationTestsBase
    {
        public OwinIntegrationTests()
            : base(new OwinItegrationSuite<Startup>(1111))
        { }
    }
}