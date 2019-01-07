using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Firestorm.FunctionalTests.Tests.Setup
{
    public class FootballTestFixture : IDisposable
    {
        private static int _startPort = 3000;

        private readonly Dictionary<FirestormApiTech, HostClientPair> _hostClientPairs;

        public FootballTestFixture()
        {
            _hostClientPairs = new Dictionary<FirestormApiTech, HostClientPair>();

            foreach (var args in new FootballHttpClientIndexes())
            {
                var tech = (FirestormApiTech) args[0];
                _hostClientPairs.Add(tech, new HostClientPair(_startPort++, tech));
            }
        }

        public HttpClient GetClient(FirestormApiTech index)
        {
            return _hostClientPairs[index].HttpClient;
        }

        public void Dispose()
        {
            foreach (HostClientPair hostClientPair in _hostClientPairs.Values)
            {
                hostClientPair.Dispose();
            }
        }
    }
}