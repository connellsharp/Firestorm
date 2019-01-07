using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class StandardStartupTests : BasicIntegrationTestsBase
    {
        public StandardStartupTests()
            : base(new KestrelIntegrationSuite<StandardStartup>(2223))
        { }
    }
}