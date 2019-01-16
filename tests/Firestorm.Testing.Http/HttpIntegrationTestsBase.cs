using System;
using System.Net.Http;

namespace Firestorm.Testing.Http
{
    public abstract class HttpIntegrationTestsBase
    {
        private readonly IHttpIntegrationSuite _integrationSuite;

        protected HttpIntegrationTestsBase(IHttpIntegrationSuite integrationSuite)
        {
            _integrationSuite = integrationSuite;
        }

        protected HttpClient HttpClient
        {
            get { return _integrationSuite.HttpClient; }
        }
    }
}