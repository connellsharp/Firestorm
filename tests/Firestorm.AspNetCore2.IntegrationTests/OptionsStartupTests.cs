using Firestorm.Testing.Http.Tests;
using JetBrains.Annotations;

namespace Firestorm.AspNetCore2.IntegrationTests
{
    [UsedImplicitly]
    public class OptionsStartupTests : BasicIntegrationTestsBase
    {
        public OptionsStartupTests()
            : base(new NetCoreIntegrationSuite<OptionsStartup>(2224))
        { }
    }
}