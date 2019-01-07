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
            _testSuite = new ExampleItegrationSuite(ExampleConfiguration.EndpointConfiguration, typeof(TTest));
            _testSuite.Start();
        }

        public HttpClient HttpClient => _testSuite.HttpClient;

        public void Dispose()
        {
            _testSuite.Dispose();
        }
    }
}