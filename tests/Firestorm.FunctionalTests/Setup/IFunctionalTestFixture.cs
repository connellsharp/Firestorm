using System;
using System.Net.Http;

namespace Firestorm.FunctionalTests.Setup
{
    public interface IFunctionalTestFixture : IDisposable
    {
        HttpClient Client { get; }
    }
}