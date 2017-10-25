using System;
using System.Net.Http;

namespace Firestorm.Tests.Integration.Http.Base
{
    public interface IHttpIntegrationSuite : IDisposable
    {
        void Start();

        HttpClient HttpClient { get; }
    }
}