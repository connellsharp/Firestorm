using System.Net.Http;

namespace Firestorm.FunctionalTests.Setup
{
    public class FunctionalTestFixture<TConfigurer> : IFunctionalTestFixture 
        where TConfigurer : IStartupConfigurer, new()
    {
        private readonly HostClientPair _pair;

        public FunctionalTestFixture()
        {
            _pair = new HostClientPair(new TConfigurer());
        }

        public HttpClient Client => _pair.HttpClient;

        public void Dispose()
        {
            _pair.Dispose();
        }
    }
}