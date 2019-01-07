using System;
using System.Net.Http;

namespace Firestorm.Testing.Http
{
    public interface IHttpIntegrationSuite : IDisposable
    {
        void Start();

        HttpClient HttpClient { get; }
    }
}