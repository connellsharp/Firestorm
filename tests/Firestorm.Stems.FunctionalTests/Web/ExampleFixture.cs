using System;
using System.Net.Http;
using Firestorm.Testing.Http;

namespace Firestorm.Stems.FunctionalTests.Web
{
    public class ExampleFixture<TTest> : IDisposable
    {
        private readonly IHttpIntegrationSuite _testSuite;

        public ExampleFixture()
        {
            _testSuite = Attempt.KeepTrying(
                () =>
                {
                    var suite = new ExampleIntegrationSuite(ExampleConfiguration.EndpointConfiguration, typeof(TTest));
                    suite.Start();
                    return suite;
                },
                new[] { 1000, 3000, 10000 });
        }

        public HttpClient HttpClient => _testSuite.HttpClient;

        public void Dispose()
        {
            _testSuite.Dispose();
        }
    }
}