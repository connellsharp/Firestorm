using System;
using System.Net.Http;

namespace Firestorm.Tests.Integration.Http.Base
{
    public abstract class HttpIntegrationTestsBase : IDisposable
    {
        private readonly IHttpIntegrationSuite _integrationSuite;

        protected HttpIntegrationTestsBase(IHttpIntegrationSuite integrationSuite)
        {
            _integrationSuite = integrationSuite;
            _integrationSuite.Start();
        }

        protected HttpClient HttpClient
        {
            get { return _integrationSuite.HttpClient; }
        }
        
        public void Dispose()
        {
            _integrationSuite.Dispose();
        }
    }
}