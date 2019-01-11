using System.Net;
using System.Net.Http;
using System.Threading;

namespace Firestorm.FunctionalTests.Setup
{
    public class FunctionalTestFixture<TConfigurer> : IFunctionalTestFixture 
        where TConfigurer : IStartupConfigurer, new()
    {
        private static readonly int[] WaitBetweenRetries = { 1000, 3000, 10000 };
        private readonly HostClientPair _pair;

        public FunctionalTestFixture()
        {
            for (int i = 0; i < WaitBetweenRetries.Length + 1; i--)
            {
                try
                {
                    _pair = new HostClientPair(new TConfigurer());
                    return;
                }
                catch (HttpListenerException) when (i < WaitBetweenRetries.Length)
                {
                    Thread.Sleep(WaitBetweenRetries[0]);
                }
            }
        }

        public HttpClient Client => _pair.HttpClient;

        public void Dispose()
        {
            _pair.Dispose();
        }
    }
}