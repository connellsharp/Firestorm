using System.IO;
using System.Net;
using Firestorm.Tests.Integration.Base.Tests;

namespace Firestorm.Tests.Integration.AspNetCore
{
    public class BasicIntegrationTests : BasicIntegrationTestsBase
    {
        public BasicIntegrationTests()
            : base(new NetCoreIntegrationSuite())
        { }
    }
}