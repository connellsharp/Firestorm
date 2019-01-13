using System.Net.Http;
using Firestorm.Testing.Http;

namespace Firestorm.FunctionalTests.Setup
{
    public class FunctionalTestFixture<TConfigurer> : IFunctionalTestFixture 
        where TConfigurer : IStartupConfigurer, new()
    {
        private readonly HostClientPair _pair;

        public FunctionalTestFixture()
        {
            _pair = Attempt.KeepTrying(
                () => new HostClientPair(new TConfigurer()),
                new[] { 1000, 3000, 10000 });
        }

        public HttpClient Client => _pair.HttpClient;

        public void Dispose()
        {
            _pair.Dispose();
        }
    }
}