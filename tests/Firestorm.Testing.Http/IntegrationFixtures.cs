using System.Collections.Generic;
using NUnit.Framework;

namespace Firestorm.Tests.Integration.Base
{
    public static class IntegrationFixtures
    {
        public static IDictionary<string, IHttpIntegrationSuite> Suites { get; } = new Dictionary<string, IHttpIntegrationSuite>();

        public static IEnumerable<TestFixtureData> Params
        {
            get
            {
                foreach (string suiteKey in Suites.Keys)
                {
                    yield return new TestFixtureData(suiteKey);
                }
            }
        }

        /* Use with :
        
        [TestFixtureSource(typeof(IntegrationFixtures), nameof(IntegrationFixtures.Params))]

        */
    }
}