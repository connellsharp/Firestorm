using System;
using System.Net.Http;
using Firestorm.Endpoints.Start;
using Firestorm.Tests.Integration.Http.Base;

namespace Firestorm.Tests.Examples.Web
{
    public class ExampleFixture<TTest> : IDisposable
    {
        private readonly IHttpIntegrationSuite _testSuite;

        public ExampleFixture()
        {
            FirestormConfiguration configuration = ExampleConfiguration.GetFirestormConfig<TTest>();
            _testSuite = new ExampleItegrationSuite(configuration);
            _testSuite.Start();
        }

        public HttpClient HttpClient => _testSuite.HttpClient;

        public void Dispose()
        {
            _testSuite.Dispose();
        }
    }
}