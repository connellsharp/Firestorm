using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Firestorm.Tests.Examples.Football.Tests
{
    public class FootballTestFixture : IDisposable
    {
        private static int _startPort = 3000;

        private readonly Dictionary<FirestormApiTech, HostClientPair> _hostClientPairs;

        public FootballTestFixture()
        {
            _hostClientPairs = new Dictionary<FirestormApiTech, HostClientPair>
            {
                { FirestormApiTech.Stems, new HostClientPair(_startPort++, FirestormApiTech.Stems)} ,
                { FirestormApiTech.Fluent, new HostClientPair(_startPort++, FirestormApiTech.Fluent)}
            };
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